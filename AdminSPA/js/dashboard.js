// Dashboard Page
const Dashboard = {
    async render() {
        try {
            const [userStats, songStats] = await Promise.all([
                api.getUserStatistics(),
                api.getSongStatistics()
            ]);

            return `
                <div class="row">
                    <div class="col-12">
                        <h2><i class="fas fa-tachometer-alt"></i> Dashboard</h2>
                        <hr>
                    </div>
                </div>

                <!-- Statistics Cards -->
                <div class="row mb-4">
                    <div class="col-md-3">
                        <div class="card text-white bg-primary">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <h6 class="card-title">Користувачі</h6>
                                        <h2 class="mb-0">${userStats.totalUsers}</h2>
                                    </div>
                                    <i class="fas fa-users fa-3x opacity-50"></i>
                                </div>
                            </div>
                            <div class="card-footer bg-transparent border-top-0">
                                <small>Активних: ${userStats.activeUsers}</small>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="card text-white bg-success">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <h6 class="card-title">Пісні</h6>
                                        <h2 class="mb-0">${songStats.totalSongs}</h2>
                                    </div>
                                    <i class="fas fa-music fa-3x opacity-50"></i>
                                </div>
                            </div>
                            <div class="card-footer bg-transparent border-top-0">
                                <small>Всього треків</small>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="card text-white bg-warning">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <h6 class="card-title">Жанри</h6>
                                        <h2 class="mb-0">${songStats.totalGenres}</h2>
                                    </div>
                                    <i class="fas fa-tags fa-3x opacity-50"></i>
                                </div>
                            </div>
                            <div class="card-footer bg-transparent border-top-0">
                                <small>Категорій</small>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3">
                        <div class="card text-white bg-info">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div>
                                        <h6 class="card-title">Адміни</h6>
                                        <h2 class="mb-0">${userStats.adminUsers}</h2>
                                    </div>
                                    <i class="fas fa-user-shield fa-3x opacity-50"></i>
                                </div>
                            </div>
                            <div class="card-footer bg-transparent border-top-0">
                                <small>Адміністраторів</small>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Charts -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5><i class="fas fa-chart-pie"></i> Пісні за жанрами</h5>
                            </div>
                            <div class="card-body">
                                <canvas id="genreChart"></canvas>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5><i class="fas fa-clock"></i> Останні пісні</h5>
                            </div>
                            <div class="card-body">
                                <div class="list-group">
                                    ${songStats.recentSongs.map(song => `
                                        <div class="list-group-item">
                                            <div class="d-flex justify-content-between align-items-center">
                                                <div>
                                                    <h6 class="mb-0">${song.title}</h6>
                                                    <small class="text-muted">${song.artist}</small>
                                                </div>
                                                <small class="text-muted">
                                                    ${new Date(song.uploadDate).toLocaleDateString('uk-UA')}
                                                </small>
                                            </div>
                                        </div>
                                    `).join('')}
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
                    Помилка завантаження даних: ${error.message}
                </div>
            `;
        }
    },

    async afterRender() {
        try {
            const songStats = await api.getSongStatistics();
            this.renderGenreChart(songStats.songsByGenre);
        } catch (error) {
            console.error('Chart render error:', error);
        }
    },

    renderGenreChart(data) {
        const ctx = document.getElementById('genreChart');
        if (!ctx) return;

        const labels = data.map(item => item.genreName);
        const values = data.map(item => item.count);

        new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: [
                        '#0d6efd', '#6610f2', '#6f42c1', '#d63384',
                        '#dc3545', '#fd7e14', '#ffc107', '#198754',
                        '#20c997', '#0dcaf0'
                    ],
                    borderWidth: 2,
                    borderColor: '#fff'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    }
};
