// Audio Player JavaScript

class AudioPlayer {
    constructor(container) {
        this.container = container;
        this.audio = container.querySelector('.audio-element');
        this.playBtn = container.querySelector('.btn-play, .btn-play-inline');
        this.progressBar = container.querySelector('.progress-bar-custom');
        this.volumeControl = container.querySelector('.volume-control');
        this.currentTimeEl = container.querySelector('.current-time');
        this.durationEl = container.querySelector('.duration');
        
        this.isPlaying = false;
        this.currentPlayer = null;
        
        this.init();
    }
    
    init() {
        // Встановлення джерела аудіо
        const songFile = this.playBtn.dataset.songFile;
        this.audio.src = `/uploads/${songFile}`;
        
        // Встановлення початкової гучності
        this.audio.volume = this.volumeControl.value / 100;
        
        // Event listeners
        this.playBtn.addEventListener('click', () => this.togglePlay());
        this.progressBar.addEventListener('input', (e) => this.seek(e));
        this.volumeControl.addEventListener('input', (e) => this.changeVolume(e));
        
        this.audio.addEventListener('loadedmetadata', () => this.updateDuration());
        this.audio.addEventListener('timeupdate', () => this.updateProgress());
        this.audio.addEventListener('ended', () => this.onEnded());
        this.audio.addEventListener('error', (e) => this.onError(e));
    }
    
    togglePlay() {
        // Зупинити всі інші плеєри
        if (AudioPlayer.currentPlayer && AudioPlayer.currentPlayer !== this) {
            AudioPlayer.currentPlayer.pause();
        }
        
        if (this.isPlaying) {
            this.pause();
        } else {
            this.play();
        }
    }
    
    play() {
        this.audio.play();
        this.isPlaying = true;
        this.playBtn.innerHTML = '<i class="fas fa-pause"></i>';
        this.playBtn.classList.add('playing');
        AudioPlayer.currentPlayer = this;
    }
    
    pause() {
        this.audio.pause();
        this.isPlaying = false;
        this.playBtn.innerHTML = '<i class="fas fa-play"></i>';
        this.playBtn.classList.remove('playing');
    }
    
    seek(e) {
        const time = (e.target.value / 100) * this.audio.duration;
        this.audio.currentTime = time;
    }
    
    changeVolume(e) {
        this.audio.volume = e.target.value / 100;
    }
    
    updateDuration() {
        const duration = this.formatTime(this.audio.duration);
        this.durationEl.textContent = duration;
    }
    
    updateProgress() {
        const progress = (this.audio.currentTime / this.audio.duration) * 100;
        this.progressBar.value = progress || 0;
        
        const currentTime = this.formatTime(this.audio.currentTime);
        this.currentTimeEl.textContent = currentTime;
    }
    
    onEnded() {
        this.isPlaying = false;
        this.playBtn.innerHTML = '<i class="fas fa-play"></i>';
        this.playBtn.classList.remove('playing');
        this.progressBar.value = 0;
        this.currentTimeEl.textContent = '0:00';
    }
    
    onError(e) {
        console.error('Audio error:', e);
        alert('Помилка завантаження аудіо файлу. Перевірте, чи файл існує.');
        this.pause();
    }
    
    formatTime(seconds) {
        if (isNaN(seconds)) return '0:00';
        
        const mins = Math.floor(seconds / 60);
        const secs = Math.floor(seconds % 60);
        return `${mins}:${secs.toString().padStart(2, '0')}`;
    }
}

// Статична властивість для відстеження поточного плеєра
AudioPlayer.currentPlayer = null;

// Ініціалізація всіх плеєрів на сторінці
document.addEventListener('DOMContentLoaded', function() {
    const playerContainers = document.querySelectorAll('.audio-player-container, .audio-player-container-inline');
    
    playerContainers.forEach(container => {
        new AudioPlayer(container);
    });
});

// Глобальна функція для додавання плеєра динамічно
window.initAudioPlayer = function(containerId) {
    const container = document.getElementById(containerId);
    if (container) {
        new AudioPlayer(container);
    }
};
