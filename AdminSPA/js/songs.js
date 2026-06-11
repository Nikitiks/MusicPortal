// Songs Page  
const Songs = {
    currentFilters: {},

    async render() {
        try {
            const [songsData, genres] = await Promise.all([
                api.getSongs(this.currentFilters),
                api.getGenres()
            ]);

            return `
                <div class="row">
                    <div class="col-12">
                        <h2><i class="fas fa-music"></i> Управління піснями</h2>
                        <hr>
                    </div>
                </div>

                <!-- Filters -->
                <div class="row mb-3">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header">
                                <h5><i class="fas fa-filter"></i> Фільтри</h5>
                            </div>
                            <div class="card-body">
                                <div class="row g-3">
                                    <div class="col-md-4">
                                        <input type="text" class="form-control" id="titleFilter" 
                                               placeholder="Пошук за назвою...">
                                    </div>
                                    <div class="col-md-4">
                                        <input type="text" class="form-control" id="artistFilter" 
                                               placeholder="Пошук за виконавцем...">
                                    </div>
                                    <div class="col-md-3">
                                        <select class="form-select" id="genreFilter">
                                            <option value="">Всі жанри</option>
                                            ${genres.map(g => `
                                                <option value="${g.id}">${g.name}</option>
                                            `).join('')}
                                        </select>
                                    </div>
                                    <div class="col-md-1">
                                        <button class="btn btn-primary w-100" onclick="Songs.applyFilters()">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Songs Table -->
                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-header d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Список пісень (${songsData.totalCount})</h5>
                                <button class="btn btn-sm btn-primary" onclick="Songs.refreshData()">
                                    <i class="fas fa-sync-alt"></i> Оновити
                                </button>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Назва</th>
                                                <th>Виконавець</th>
                                                <th>Жанр</th>
                                                <th>Користувач</th>
                                                <th>Дата</th>
                                                <th>Дії</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${songsData.songs.map(song => `
                                                <tr>
                                                    <td>${song.id}</td>
                                                    <td>
                                                        <i class="fas fa-music text-primary"></i> 
                                                        <strong>${song.title}</strong>
                                                    </td>
                                                    <td>${song.artist}</td>
                                                    <td>
                                                        <span class="badge bg-primary">${song.genreName || 'N/A'}</span>
                                                    </td>
                                                    <td>${song.username || 'N/A'}</td>
                                                    <td>${new Date(song.uploadDate).toLocaleDateString('uk-UA')}</td>
                                                    <td>
                                                        <button class="btn btn-sm btn-danger" 
                                                                onclick="Songs.confirmDelete(${song.id}, '${song.title}')">
                                                            <i class="fas fa-trash"></i>
                                                        </button>
                                                    </td>
                                                </tr>
                                            `).join('')}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        } catch (error) {
            return `
                <div class="alert alert-danger">
                    <i class="fas fa-exclamation-triangle"></i> 
                    Помилка завантаження пісень: ${error.message}
                </div>
            `;
        }
    },

    applyFilters() {
        this.currentFilters = {
            title: document.getElementById('titleFilter')?.value || '',
            artist: document.getElementById('artistFilter')?.value || '',
            genreId: document.getElementById('genreFilter')?.value || ''
        };
        this.refreshData();
    },

    confirmDelete(id, title) {
        showConfirmModal(
            `Ви впевнені, що хочете видалити пісню "${title}"?`,
            () => this.deleteSong(id)
        );
    },

    async deleteSong(id) {
        try {
            await api.deleteSong(id);
            showToast('Пісню видалено', 'success');
            await this.refreshData();
        } catch (error) {
            showToast('Помилка: ' + error.message, 'danger');
        }
    },

    async refreshData() {
        await loadPage('songs');
    }
};
