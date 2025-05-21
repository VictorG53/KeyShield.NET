let dotNetReference = null;

window.initFormSubmission = function (dotNetRef) {
    dotNetReference = dotNetRef;
};

window.generateSalt = function () {
    return window.crypto.getRandomValues(new Uint8Array(16));
}

window.hashPassword = async function (password) {
    const enc = new TextEncoder();
    const passwordData = enc.encode(password);

    const hashBuffer = await window.crypto.subtle.digest(
        'SHA-256',
        passwordData
    );

    return new Uint8Array(hashBuffer);
};

window.getAndHashPassword = async function () {
    const passwordElement = document.getElementById('coffrePassword');
    if (!passwordElement || !passwordElement.value) {
        alert('Le mot de passe est obligatoire.');
        return;
    }

    const password = passwordElement.value;
    try {
        return await window.hashPassword(password);
    } catch (error) {
        console.error('Error hashing password:', error);
        return null;
    }
};


