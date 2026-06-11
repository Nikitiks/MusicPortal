// Genres Page
const Genres = {
    async render() {
        try {
            const genres = await api.getGenres();

            return `
                <div class="row">
                    <div class="col-12">
                        <h2><i class="fas fa-tags"></i> Управління жанрами</h2>
                        <hr>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-8">
                        <div class="card">
                            <div class="card-header d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Список жанрів</h5>
                                <button class="btn btn-sm btn-success" onclick="Genres.showCreateModal()">
                                    <i class="fas fa-plus"></i> Додати жанр
                                </button>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Назва</th>
                                                <th>Кількість пісень</th>
                                                <th>Дії</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${genres.map(genre => `
                                                <tr>
                                                    <td>${genre.id}</td>
                                                    <td>
                                                        <i class="fas fa-tag text-primary"></i> 
                                                        <strong>${genre.name}</strong>
                                                    </td>
                                                    <td>
                                                        <span class="badge bg-info">${genre.songsCount} пісень</span>
                                                    </td>
                                                    <td>
                                                        <button class="btn btn-sm btn-warning" 
                                                                onclick="Genres.showEditModal(${genre.id}, '${genre.name}')">
                                                            <i class="fas fa-edit"></i>
                                                        </button>
                                                        <button class="btn btn-sm btn-danger" 
                                                                onclick="Genres.confirmDelete(${genre.id}, '${genre.name}', ${genre.songsCount})"
                                                                ${genre.songsCount > 0 ? 'disabled' : ''}>
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

                    <div class="col-md-4">
                        <div class="card">
                            <div class="card-header">
                                <h5><i class="fas fa-info-circle"></i> Інформація</h5>
                            </div>
                            <div class="card-body">
                                <p><strong>Всього жанрів:</strong> ${genres.length}</p>
                                <p><strong>Жанрів з піснями:</strong> ${genres.filter(g => g.songsCount > 0).length}</p>
                                <hr>
                                <p class="small text-muted">
                                    <i class="fas fa-lightbulb"></i> 
                                    Жанр можна видалити тільки якщо він не містить пісень.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        } catch (error) {
            return `
                <div class="alert alert-danger">
                    <i class="fas fa-exclamation-triangle"></i> 
                    Помилка завантаження жанрів: ${error.message}
                </div>
            `;
        }
    },

    showCreateModal() {
        document.getElementById('genreModalTitle').textContent = 'Додати жанр';
        document.getElementById('genreId').value = '';
        document.getElementById('genreName').value = '';
        document.getElementById('genreName').classList.remove('is-invalid');
        
        const modal = new bootstrap.Modal(document.getElementById('genreModal'));
        modal.show();
    },

    showEditModal(id, name) {
        document.getElementById('genreModalTitle').textContent = 'Редагувати жанр';
        document.getElementById('genreId').value = id;
        document.getElementById('genreName').value = name;
        document.getElementById('genreName').classList.remove('is-invalid');
        
        const modal = new bootstrap.Modal(document.getElementById('genreModal'));
        modal.show();
    },

    async saveGenre() {
        const id = document.getElementById('genreId').value;
        const name = document.getElementById('genreName').value.trim();
        const nameInput = document.getElementById('genreName');

        // Валідація
        if (!name) {
            nameInput.classList.add('is-invalid');
            return;
        }

        try {
            if (id) {
                // UPDATE (PUT)
                await api.updateGenre(id, { name });
                showToast('Жанр оновлено', 'success');
            } else {
                // CREATE (POST)
                await api.createGenre({ name });
                showToast('Жанр додано', 'success');
            }

            // Закрити modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('genreModal'));
            modal.hide();

            // Оновити дані
            await this.refreshData();
        } catch (error) {
            showToast('Помилка: ' + error.message, 'danger');
        }
    },

    confirmDelete(id, name, songsCount) {
        if (songsCount > 0) {
            showToast('Не можна видалити жанр з піснями', 'warning');
            return;
        }

        showConfirmModal(
            `Ви впевнені, що хочете видалити жанр "${name}"?`,
            () => this.deleteGenre(id)
        );
    },

    async deleteGenre(id) {
        try {
            await api.deleteGenre(id);
            showToast('Жанр видалено', 'success');
            await this.refreshData();
        } catch (error) {
            showToast('Помилка: ' + error.message, 'danger');
        }
    },

    async refreshData() {
        await loadPage('genres');
    }
};

// Event listener для кнопки збереження жанру
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('saveGenreBtn')?.addEventListener('click', () => {
        Genres.saveGenre();
    });
});
