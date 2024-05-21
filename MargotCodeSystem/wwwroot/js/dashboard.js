function showResidents() {
    fetchFilteredData("Residents");
}

function showSeniorCitizens() {
    fetchFilteredData("SeniorCitizens");
}

function showMedicationUsers() {
    fetchFilteredData("MedicationUsers");
}

function showStreetSweepers() {
    fetchFilteredData("StreetSweepers");
}

function showPetOwners() {
    fetchFilteredData("PetOwners");
}

function showHouseOccupants() {
    fetchFilteredData("HouseOccupants");
}

function fetchFilteredData(category) {
    fetch(`/Resident/GetFilteredResidents?category=${category}`)
        .then(response => response.json())
        .then(data => {

            updateUI(data);
        })
        .catch(error => console.error('Error fetching data:', error));
}

function updateUI(data) {
    const contentDiv = document.getElementById("dynamicContent");
    contentDiv.innerHTML = data;
}
