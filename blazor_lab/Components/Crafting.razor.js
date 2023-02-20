window.Crafting =
{
    AddActions: function (data) {

        data.forEach(element => {
            const div = document.createElement('div');
            div.innerHTML = 'Action: ' + element.action + ' - Index: ' + element.index;

            if (element.item) {
                div.innerHTML += ' - Item Name: ' + element.item.name;
            }

            document.getElementById('actions').appendChild(div);
        });
    }
}