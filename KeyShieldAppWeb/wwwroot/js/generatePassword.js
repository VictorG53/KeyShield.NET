function generateRandomPassword(length = 16, useUpper = true, useSpecial = true) {
    let charset = "abcdefghijklmnopqrstuvwxyz0123456789";
    if (useUpper) charset += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    if (useSpecial) charset += "!@#$%^&*()_+-=[]{}|;:,.<>?";

    const randomValues = window.crypto.getRandomValues(new Uint8Array(length));
    let password = "";
    for (let i = 0; i < length; ++i) {
        password += charset.charAt(randomValues[i] % charset.length);
    }
    return password;
}

function setPasswordField(password, fieldId) {
    const field = document.getElementById(fieldId);
    if (field) {
        field.value = password;
        field.dispatchEvent(new Event('input'));
    }
}