// Main App Controller - Single Page Application
const App = {
    pages: {
        dashboard: Dashboard,
        users: Users,
        songs: Songs,
        genres: Genres
    },

    currentPage: 'dashboard',

    async init() {
        // Перевірка зв'язку з API
        await this.checkApiStatus();

        // Navigation listeners
        this.setupNavigation();

        // Load default page
        await this.loadPage('dashboard');

        // Setup modal event listeners
        this.setupModals();
    },

    setupNavigation() {
        document.querySelectorAll('[data-page]').forEach(link => {
            link.addEventListener('click', async (e) => {
                e.preventDefault();
                const page = e.currentTarget.dataset.page;
                await this.loadPage(page);

                // Update active nav item
                document.querySelectorAll('.nav-link').forEach(nav => nav.classList.remove('active'));
                e.currentTarget.classList.add('active');
            });
        });
    },

    async loadPage(pageName) {
        if (!this.pages[pageName]) {
            console.error('Page not found:', pageName);
            return;
        }

        this.currentPage = pageName;
        const appContainer = document.getElementById('app');

        // Show loading
        appContainer.innerHTML = `
            <div class="text-center py-5">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Завантаження...</span>
                </div>
                <p class="mt-3">Завантаження...</p>
            </div>
        `;

        try {
            // Render page
            const page = this.pages[pageName];
            const html = await page.render();
            appContainer.innerHTML = html;

            // After render hook
            if (page.afterRender) {
                await page.afterRender();
            }
        } catch (error) {
            appContainer.innerHTML = `
                <div class="alert alert-danger">
                    <h4><i class="fas fa-exclamation-triangle"></i> Помилка</h4>
                    <p>${error.message}</p>
                    <button class="btn btn-primary" onclick="App.loadPage('${pageName}')">
                        <i class="fas fa-sync-alt"></i> Спробувати ще раз
                    </button>
                </div>
            `;
        }
    },

    async checkApiStatus() {
        const statusBadge = document.getElementById('apiStatus');
        
        try {
            const isHealthy = await api.checkHealth();
            
            if (isHealthy) {
                statusBadge.textContent = 'Online';
                statusBadge.className = 'badge bg-success';
            } else {
                statusBadge.textContent = 'Offline';
                statusBadge.className = 'badge bg-danger';
            }
        } catch (error) {
            statusBadge.textContent = 'Error';
            statusBadge.className = 'badge bg-danger';
        }

        // Check every 30 seconds
        setTimeout(() => this.checkApiStatus(), 30000);
    },

    setupModals() {
        // Save genre button
        document.getElementById('saveGenreBtn')?.addEventListener('click', () => {
            if (typeof Genres !== 'undefined') {
                Genres.saveGenre();
            }
        });
    }
};

// Helper Functions
function showToast(message, type = 'info') {
    const toast = document.getElementById('toast');
    const toastBody = toast.querySelector('.toast-body');
    const toastHeader = toast.querySelector('.toast-header');
    
    // Set message
    toastBody.textContent = message;
    
    // Set color based on type
    const colors = {
        success: 'text-bg-success',
        danger: 'text-bg-danger',
        warning: 'text-bg-warning',
        info: 'text-bg-info'
    };
    
    toast.className = `toast ${colors[type] || colors.info}`;
    
    // Show toast
    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();
}

function showConfirmModal(message, onConfirm) {
    const modal = document.getElementById('confirmModal');
    const messageEl = document.getElementById('confirmMessage');
    const confirmBtn = document.getElementById('confirmBtn');
    
    messageEl.textContent = message;
    
    // Remove old listeners
    const newConfirmBtn = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);
    
    // Add new listener
    document.getElementById('confirmBtn').addEventListener('click', async () => {
        const bsModal = bootstrap.Modal.getInstance(modal);
        bsModal.hide();
        
        if (onConfirm) {
            await onConfirm();
        }
    });
    
    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}

// Global loadPage function
async function loadPage(pageName) {
    await App.loadPage(pageName);
}

// Initialize app when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    App.init();
});
