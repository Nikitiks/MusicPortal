// Users Page
const Users = {
    async render() {
        try {
            const users = await api.getUsers();

            return `
                <div class="row">
                    <div class="col-12">
                        <h2><i class="fas fa-users"></i> Управління користувачами</h2>
                        <hr>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-header d-flex justify-content-between align-items-center">
                                <h5 class="mb-0">Список користувачів</h5>
                                <button class="btn btn-sm btn-primary" onclick="Users.refreshData()">
                                    <i class="fas fa-sync-alt"></i> Оновити
                                </button>
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>ID</th>
                                                <th>Username</th>
                                                <th>Email</th>
                                                <th>Статус</th>
                                                <th>Роль</th>
                                                <th>Пісень</th>
                                                <th>Дата реєстрації</th>
                                                <th>Дії</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${users.map(user => `
                                                <tr>
                                                    <td>${user.id}</td>
                                                    <td>
                                                        <i class="fas fa-user"></i> ${user.username}
                                                    </td>
                                                    <td>${user.email}</td>
                                                    <td>
                                                        <span class="badge ${user.isActive ? 'bg-success' : 'bg-secondary'}">
                                                            ${user.isActive ? 'Активний' : 'Неактивний'}
                                                        </span>
                                                    </td>
                                                    <td>
                                                        ${user.isAdmin ? 
                                                            '<span class="badge bg-danger"><i class="fas fa-shield-alt"></i> Admin</span>' : 
                                                            '<span class="badge bg-info">User</span>'}
                                                    </td>
                                                    <td>${user.songsCount}</td>
                                                    <td>${new Date(user.createdAt).toLocaleDateString('uk-UA')}</td>
                                                    <td>
                                                        <button class="btn btn-sm btn-warning" 
                                                                onclick="Users.toggleStatus(${user.id})"
                                                                ${user.isAdmin ? 'disabled' : ''}>
                                                            <i class="fas fa-toggle-on"></i>
                                                        </button>
                                                        <button class="btn btn-sm btn-danger" 
                                                                onclick="Users.confirmDelete(${user.id}, '${user.username}')"
                                                                ${user.isAdmin ? 'disabled' : ''}>
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
                    Помилка завантаження користувачів: ${error.message}
                </div>
            `;
        }
    },

    async toggleStatus(id) {
        try {
            await api.toggleUserStatus(id);
            showToast('Статус користувача змінено', 'success');
            await this.refreshData();
        } catch (error) {
            showToast('Помилка: ' + error.message, 'danger');
        }
    },

    confirmDelete(id, username) {
        showConfirmModal(
            `Ви впевнені, що хочете видалити користувача "${username}"?`,
            () => this.deleteUser(id)
        );
    },

    async deleteUser(id) {
        try {
            await api.deleteUser(id);
            showToast('Користувача видалено', 'success');
            await this.refreshData();
        } catch (error) {
            showToast('Помилка: ' + error.message, 'danger');
        }
    },

    async refreshData() {
        await loadPage('users');
    }
};
