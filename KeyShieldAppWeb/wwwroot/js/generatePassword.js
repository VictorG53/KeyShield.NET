function generateRandomPassword(length = 16, useUpper = true, useSpecial = true) {
    let charset = "abcdefghijklmnopqrstuvwxyz0123456789";
    if (useUpper) charset += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    if (useSpecial) charset += "!@#$%^&*()_+-=[]{}|;:,.<>?";

    const randomValues = window.crypto.getRandomValues(new Uint8Array(length));
    let password = "";
    for (let i = 0; i < length; ++i) {
        password += charset.charAt(randomValues[i] % charset.length);
    }

    document.getElementById('generatedPassword').value = password;
    return password;
}

function setPasswordField(password, fieldId) {
    const field = document.getElementById(fieldId);
    if (field) {
        field.value = password;
        field.dispatchEvent(new Event('input'));
    }
}

function checkPasswordStrength(password) {
    let strength = 0;
    if (password.length >= 8) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;

    if (strength <= 2) return "Faible";
    if (strength === 3 || strength === 4) return "Moyen";
    if (strength === 5) return "Fort";
    return "Très faible";
}

function showStrength(e) {
    let input = e && e.target ? e.target : document.activeElement;
    if (!input || !input.id) return;

    let pwd = input.value;
    let strength = checkPasswordStrength(pwd);

    // Cherche le <small> juste après l'input pour afficher la force
    let strengthEl = input.parentElement.querySelector('small.form-text');
    if (!strengthEl) {
        // Si pas trouvé, cherche par id
        strengthEl = document.getElementById('passwordStrength') || document.getElementById(input.id + 'Strength');
    }
    if (strengthEl) {
        strengthEl.textContent = "Force du mot de passe : " + strength;
        strengthEl.style.color = strength === "Fort" ? "green" : (strength === "Moyen" ? "orange" : "red");
    }
}