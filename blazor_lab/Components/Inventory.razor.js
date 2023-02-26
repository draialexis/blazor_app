window.Inventory =
{
    AddActions: function (data) {

        data.forEach(element => {
            const div = document.createElement('div');
            div.innerHTML = 'Action: ' + element.action + ' - Position: ' + element.position;

            if (element.inventoryModel) {
                div.innerHTML += ' - Item Name: ' + element.inventoryModel.itemName;
            }

            document.getElementById('actions').appendChild(div);
        });
    }
}