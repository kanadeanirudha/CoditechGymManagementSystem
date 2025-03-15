var GymSalesInvoice = {
    Initialize: function () {
        GymSalesInvoice.constructor();
    },
    constructor: function () {
    },

    GetSaleInvoiceListByCentreCode: function (controllerName, methodName) {
        $("#SelectedParameter1").val($("#FromDate").val());
        $("#SelectedParameter2").val($("#ToDate").val());
        CoditechCommon.LoadListByCentreCode(controllerName, methodName);
    },

    Print: function () {
        var divContents = document.getElementById("PrintSalesInvoiceDivId").innerHTML;
        var printWindow = window.open('', '', 'height=200,width=400');
        printWindow.document.write('<html><head><title>Receipt</title>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(divContents);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    },
}
