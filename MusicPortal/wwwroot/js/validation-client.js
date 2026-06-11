// Кліє ClientValidation.js
/**
 * Розширена клієнтська валідація для форм
 */

$(document).ready(function () {
    // Додавання класу shake при помилці валідації
    $('form').on('invalid-form.validate', function () {
        $(this).find('.input-validation-error').addClass('shake');
        setTimeout(function () {
            $('.input-validation-error').removeClass('shake');
        }, 500);
    });

    // Валідація розміру файлу
    $('input[type="file"]').on('change', function () {
        const maxSize = 10 * 1024 * 1024; // 10 MB
        const file = this.files[0];
        const errorElement = $(this).next('.field-validation-error');
        
        if (file && file.size > maxSize) {
            const maxSizeMB = maxSize / 1024 / 1024;
            if (errorElement.length === 0) {
                $(this).after(`<span class="field-validation-error">Максимальний розмір файлу: ${maxSizeMB} MB</span>`);
            } else {
                errorElement.text(`Максимальний розмір файлу: ${maxSizeMB} MB`);
            }
            $(this).addClass('input-validation-error');
            $(this).val('');
        } else if (errorElement.length > 0 && !errorElement.data('valmsg-for')) {
            errorElement.remove();
            $(this).removeClass('input-validation-error');
        }
    });

    // Валідація розширення файлу
    $('input[type="file"]').on('change', function () {
        const allowedExtensions = ['.mp3', '.wav', '.flac', '.m4a'];
        const file = this.files[0];
        
        if (file) {
            const fileName = file.name.toLowerCase();
            const isValidExtension = allowedExtensions.some(ext => fileName.endsWith(ext));
            
            if (!isValidExtension) {
                alert(`Дозволені розширення файлів: ${allowedExtensions.join(', ')}`);
                $(this).val('');
                $(this).addClass('input-validation-error');
            } else {
                $(this).removeClass('input-validation-error');
            }
        }
    });

    // Показ імені файлу після вибору
    $('input[type="file"]').on('change', function () {
        const fileName = $(this).val().split('\\').pop();
        const fileLabel = $(this).siblings('.file-label');
        
        if (fileName && fileLabel.length > 0) {
            fileLabel.text(fileName);
        }
    });

    // Валідація паролів на співпадіння в реальному часі
    $('input[data-val-equalto]').on('keyup', function () {
        const confirmPassword = $(this);
        const password = $(confirmPassword.data('val-equalto-other').replace('*.', '#'));
        const errorSpan = confirmPassword.siblings('.field-validation-error');
        
        if (confirmPassword.val() !== '' && password.val() !== confirmPassword.val()) {
            if (!errorSpan.length || !errorSpan.is(':visible')) {
                confirmPassword.addClass('input-validation-error');
            }
        } else if (confirmPassword.val() === password.val()) {
            confirmPassword.removeClass('input-validation-error');
        }
    });

    // Додавання required індикатора до label
    $('input[data-val-required], select[data-val-required], textarea[data-val-required]').each(function () {
        const input = $(this);
        const label = $(`label[for="${input.attr('id')}"]`);
        
        if (label.length > 0 && !label.hasClass('required')) {
            label.addClass('required');
        }
    });

    // Валідація email в реальному часі
    $('input[type="email"]').on('blur', function () {
        const email = $(this).val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const errorSpan = $(this).siblings('.field-validation-error');
        
        if (email !== '' && !emailRegex.test(email)) {
            $(this).addClass('input-validation-error');
            if (errorSpan.length === 0) {
                $(this).after('<span class="field-validation-error">Некоректний формат email</span>');
            }
        } else if (emailRegex.test(email) || email === '') {
            $(this).removeClass('input-validation-error');
            if (errorSpan.length > 0 && !errorSpan.data('valmsg-for')) {
                errorSpan.remove();
            }
        }
    });

    // Очищення помилок при фокусі
    $('input, select, textarea').on('focus', function () {
        $(this).removeClass('input-validation-error');
    });

    // Прокрутка до першої помилки валідації
    $('form').on('submit', function (e) {
        const firstError = $(this).find('.input-validation-error:first');
        
        if (firstError.length > 0) {
            $('html, body').animate({
                scrollTop: firstError.offset().top - 100
            }, 500);
            firstError.focus();
        }
    });

    // Валідація форми перед submit
    $('form').on('submit', function (e) {
        const form = $(this);
        
        // Перевірка jQuery validation
        if (form.valid && !form.valid()) {
            e.preventDefault();
            return false;
        }

        // Додаткова перевірка required полів
        let hasErrors = false;
        form.find('input[required], select[required], textarea[required]').each(function () {
            if ($(this).val() === '' || $(this).val() === null) {
                $(this).addClass('input-validation-error');
                hasErrors = true;
            }
        });

        if (hasErrors) {
            e.preventDefault();
            return false;
        }
    });

    // Автоматичне видалення пробілів з username
    $('input[name="Username"]').on('blur', function () {
        $(this).val($(this).val().trim().replace(/\s+/g, ''));
    });

    // Показ/приховування пароля
    $('.toggle-password').on('click', function () {
        const input = $($(this).data('target'));
        const icon = $(this).find('i');
        
        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
            icon.removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            input.attr('type', 'password');
            icon.removeClass('fa-eye-slash').addClass('fa-eye');
        }
    });

    // Прогрес бар для strength password (якщо потрібно)
    $('input[type="password"][name="Password"]').on('keyup', function () {
        const password = $(this).val();
        let strength = 0;
        
        if (password.length >= 6) strength++;
        if (password.match(/[a-z]+/)) strength++;
        if (password.match(/[A-Z]+/)) strength++;
        if (password.match(/[0-9]+/)) strength++;
        if (password.match(/[$@#&!]+/)) strength++;
        
        const strengthBar = $(this).siblings('.password-strength');
        if (strengthBar.length > 0) {
            strengthBar.removeClass('weak medium strong very-strong');
            
            if (strength <= 2) {
                strengthBar.addClass('weak').css('width', '33%');
            } else if (strength === 3) {
                strengthBar.addClass('medium').css('width', '66%');
            } else if (strength === 4) {
                strengthBar.addClass('strong').css('width', '85%');
            } else {
                strengthBar.addClass('very-strong').css('width', '100%');
            }
        }
    });
});
