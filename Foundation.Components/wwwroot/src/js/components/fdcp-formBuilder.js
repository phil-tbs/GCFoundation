document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll('[data-dependencies]').forEach(function (el) {

        el.addEventListener("gcdsChange", (event) => {
            const dependencies = JSON.parse(el.getAttribute('data-dependencies'));

            dependencies.forEach((dependency) => {
                if (dependency.triggerValue === event.target.value) {

                    let targetElement = document.querySelector(`[input-id="${dependency.targetQuestionId}"]`);
                    if (dependency.action === 'Require') {
                        toggleRequired(targetElement);
                    }


                }
            });
        });
    });
});

const toggleRequired = (element) => {
    console.log("required");
    let currentValue = element.value;

    const clone = element.cloneNode(true);
    clone.required = !element.required;
    clone.value = currentValue; // Retain value
    element.parentNode.replaceChild(clone, element);
};

const toggleDisable = (element) => {

}



const applyDependencies = (target, action) => {
    let targetElement = document.querySelector(`[input-id="${target}"]`);
    if (!targetElement) return;

    let currentValue = targetElement.value;

    originalValue.set("target", currentValue);

    console.log("saved current value: " + currentValue);


    // If you need to recreate the element (e.g., for 'require'), do so and keep value if needed
    if (action === 'Require' || action === 'Disable') {
        console.log("replace")
        const clone = targetElement.cloneNode(true);
        clone.required = true;
        //clone.disabled = targetElement.disabled;
        clone.value = currentValue; // Retain value
        targetElement.parentNode.replaceChild(clone, targetElement);
    }
}
