window.addEventListener('DOMContentLoaded', async () => {
    await getFunctionApiUrl();
    getVistCount();
});

let functionApiUrl = '';

const getFunctionApiUrl = async () => {
    try {
        const response = await fetch('https://getresumecounternish.azurewebsites.net/api/GetFunctionApiUrl');
        const data = await response.json();
        functionApiUrl = data.url;
    } catch (error) {
        console.error("Error fetching Function API URL:", error);
    }
};

const getVistCount = async () => {
    if (!functionApiUrl) {
        console.error("Function API URL not set.");
        return;
    }

    try {
        const response = await fetch(functionApiUrl);
        const data = await response.json();
        console.log("Website called function API");
        document.getElementById("counter").innerText = data.count;
    } catch (error) {
        console.error(error);
    }
};
