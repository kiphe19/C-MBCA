var editor;

function getAV() {
    $.post("api/dataa")
            .success(function (e) {
                var activity = new Array();
                for (var i in e.data) {
                    var qp = {
                        label: e.data[i].activity_name,
                        value: e.data[i].activity_name
                    }
                    activity.push(qp);
                }
                dailyEditor.field("daily_activity").update(activity);
            })
    $.post("api/datav")
            .success(function (e) {
                var vessel = new Array();
                for (var i in e.data) {
                    var qp = {
                        label: e.data[i].name,
                        value: e.data[i].name
                    }
                    vessel.push(qp);
                }
                dailyEditor.field("daily_vessel").update(vessel);
            })
}


(function () {

    dailyEditor = new $.fn.dataTable.Editor({
        ajax: "api/daily",
        table: "#dailyTable",
        fields: [
            { label: "Date", name: "daily_date", type: "datetime", format: "MM/DD/YYYY" },
            { label: "Vessel", name: "daily_vessel", type: "select" },
            { label: "Activity", name: "daily_activity", type: "select" },
            { label: "Duration", name: "daily_duration" }
        ]
    }).one('preOpen', function () {
        getAV();
    }).on('preOpen', function () {
        dailyEditor.title('Add new Daily Activity');
    })

    $("#dailyTable").DataTable({
        dom: 'Bfrtip',
        ajax: {
            url: "api/daily",
            method: "post"
        },
        serverSide: true,
        deferRender: true,
        columns: [
            { data: "daily_date" },
            { data: "daily_vessel" },
            { data: "daily_activity" },
            { data: "daily_duration" }
        ],
        select: true,
        buttons: [
            { extend: "create", editor: dailyEditor, text: "Add new Daily Activity" },
            { extend: "edit", editor: dailyEditor },
            { extend: "remove", editor: dailyEditor },
            //{
            //    extend: "collection",
            //    text: "<i class='glyphicon-export'> Export to ..<i>",
            //    buttons: [
            //        'excel'
            //    ]
            //},
            {
                text: "Save data today",
                action: function (e, dt, node, config) {
                    alert("click");
                }

            },
        ]
    });


    editor = new $.fn.dataTable.Editor({
        ajax: "api/monthly",
        table: "#monthlyTable",
        fields: [
            { label: "Date", name: "monthly_date", type: "datetime", format: "MM/DD/YYYY" },
            { label: "Vessel", name: "monthly_vessel", type: "select" },
            { label: "Activity", name: "monthly_activity", type: "select" },
            { label: "Duration", name: "monthly_duration" }
        ]
    })

    $("#monthlyTable").dataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/monthly",
            type: "post"
        },
        serverSide: true,
        columns: [
            { data: "monthly_date" },
            { data: "monthly_vessel" },
            { data: "monthly_activity" },
            { data: "monthly_duration" }
        ],
        select: true,
        buttons: [
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor },
            {
                extend: "collection",
                text: "Export to ..",
                buttons: [
                    'excel'
                ]
            }
        ]
    });
})(jQuery)