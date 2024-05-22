function showResidents() {
    document.getElementById('dynamicContent').innerHTML = '<h1>Residents Content</h1><p>This is the content for residents.</p>';
    clearClicked();
    document.getElementById('residents').classList.add('clicked');
}

function showSeniorCitizens() {
    document.getElementById('dynamicContent').innerHTML = '<h1>Senior Citizens Content</h1><p>This is the content for senior citizens.</p>';
    clearClicked();
    document.getElementById('seniorCitizens').classList.add('clicked');
}

function showMedicationUsers() {
    document.getElementById('dynamicContent').innerHTML = '<h1>Medication Users Content</h1><p>This is the content for medication users.</p>';
    clearClicked();
    document.getElementById('medicationUsers').classList.add('clicked');
}

function showStreetSweepers() {
    document.getElementById('dynamicContent').innerHTML = '<h1>Street Sweepers Content</h1><p>This is the content for street sweepers.</p>';
    clearClicked();
    document.getElementById('streetSweepers').classList.add('clicked');
}

function showPetOwners() {
    document.getElementById('dynamicContent').innerHTML = '<h1>Pet Owners Content</h1><p>This is the content for pet owners.</p>';
    clearClicked();
    document.getElementById('petOwners').classList.add('clicked');
}
function showHouseOccupants() {
    document.getElementById('dynamicContent').innerHTML = '<h1>House Occupants Content</h1><p>This is the content for pet owners.</p>';
    clearClicked();
    document.getElementById('houseOccupants').classList.add('clicked');
}


function clearClicked() {
    const clickedElements = document.querySelectorAll('.clicked');
    clickedElements.forEach(element => {
        element.classList.remove('clicked');
    });
}