$(function () {
    $('.money').keyup(function (event) {
        var val = this.value;
        val = val.replace(/[^0-9\.]/g, '');

        if (val != "") {
            valArr = val.split('.');
            valArr[0] = (parseInt(valArr[0], 10)).toLocaleString();
            val = valArr.join('.');
        }

        this.value = val;
    });
});
