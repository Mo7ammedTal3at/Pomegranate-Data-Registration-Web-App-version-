//$(document).ready(function () {
//    $('#customers').DataTable({
//        dom: 'Bfrtip',
//        buttons: [
//            {
//                extend: 'print',
//                messageTop: 'This print was produced using the Print button for DataTables'
//            }
//        ]
//    });
//});
$(document).ready(function () {
    $('#customers').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body)
                        .css('font-size', '10pt')
                        .prepend(
                            '<link rel="stylesheet" href="../../content/printStyle.css"/>'
                        );

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]
    });
});