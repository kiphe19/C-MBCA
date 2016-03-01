var editor,
    unitEditor,
    unitTable,
    HireEditor,
    i = 1;

function SetSelectedIndex(dropdownlist, sVal) {
    var a = $(dropdownlist)[0];

    for (i = 0; i < a.options.length; i++) {
        if (a.options[i].value == sVal) {
            a.selectedIndex = i;
        }
    }
}

$(document).ready(function () {
    editor = new $.fn.dataTable.Editor({
        ajax: "api/vessel",
        table: "#vesselTable",
        fields: [
            {
                label: "Vessel Name",
                name: "name",
            },
            {
                label: "Description",
                name: "vs_desc"
            }
        ]
    }).on('initCreate', function () {
        i = vesselTable.data().length + 1;
    }).on('remove', function () {
        i = 1;
        vesselTable.ajax.reload()
    }).on('edit', function () {
        i = 1;
        vesselTable.ajax.reload()
    })

    vesselTable = $('#vesselTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/vessel",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "name" },
            { data: "vs_desc" }
        ],
        select: true,
        buttons: [
            { extend: 'create', editor: editor, text: "Add New Vessel" },
            { extend: 'edit', editor: editor },
            { extend: 'remove', editor: editor },
        ]
    }).on('init', function () {
        i = 1;
    })

    editor = new $.fn.dataTable.Editor({
        ajax: "api/activity",
        table: "#activityTable",
        fields: [
            { label: "Activity Name", name: "activity_name" },
            { label: "Description", name: "activity_ket" }
        ]
    });

    $('#activityTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/activity",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "activity_name" },
            { data: "activity_ket" }
        ],
        select: true,
        buttons: [
            {
                extend: 'create',
                editor: editor,
                text: "Add New Activity",
            },
            {
                extend: 'edit',
                editor: editor
            },
            {
                extend: 'remove',
                editor: editor,
            },
        ]
    }).on('init', function () {
        i = 1;
    });


    unitEditor = new $.fn.dataTable.Editor({
        ajax: "api/unit",
        table: "#bargeTable"
    });

    unitTable = $("#bargeTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/unit",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "unit_name" },
            //{ data: "unit_cat" },
            { data: "unit_distance" },
            { data: "unit_ket" }
        ],
        select: true,
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalBarge").modal({ backdrop: false });
                    $("#modalBarge button[type='submit']").text("Add")
                    $("#modalBarge input").val(null);
                    $("#modalBarge input[name='action']").val("create");
                }
            //},
            //{
            //    text: "Edit",
            //    action: function () {
            //        var a = unitTable.rows('.selected').indexes();
            //        var b = unitTable.row(a).data()
            //        if (a.length > 0) {
            //            $("#modalBarge").modal({ backdrop: false })
            //            $("#modalBarge input[name='action']").val("update")
            //            $("#modalBarge button[type='submit']").text("Update")
            //            $("#modalBarge input[name='unit_name']").val(b.unit_name)
            //            $("#modalBarge input[name='id']").val(b.id)
            //            $("#modalBarge select[name='unit_cat'").val(b.unit_cat)
            //            $("#modalBarge select[id='distance'").val(b.unit_distance)
            //            $("#modalBarge input[name='unit_desc'").val(b.unit_ket)
            //        }
            //    }
            },
            { extend: 'remove', editor: unitEditor }
        ]
    }).on('init', function () {
        i = 1;
    });

    mainunittable = $('#mainunitTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/mainunit",
            type: "post"
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            {data:"nama"}
        ],
        select: true,
        buttons: [
            { extend: "create", editor: editor, text: "Create new Main User" },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });


    editor = new $.fn.dataTable.Editor({
        ajax: "api/distance",
        table: "#distanceTable",
        fields: [
            { label: "Area Name", name: "distance_name" },
            { label: "Distance", name: "distance" }
        ]
    })
    editor.on('edit', function () {
        getNewDistance();
    })

    $("#distanceTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/distance",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "distance_name" },
            {
                data: null, render: function (data, type, row) {
                    return data.distance + " NMi";
                }
            }
        ],
        select: true,
        buttons: [
            { extend: "create", editor: editor, text: "Create new Area" },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/fuel",
        table: "#fuelTable"
    })

    fuelTable = $("#fuelTable").DataTable({
        dom: "Bfrtip",
        //"order": [[ 2, "desc" ]],
        ajax: {
            url: "api/fuel",
            type: "post"
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "tgl" },
            {
                data: null, render: function (data, type, row) {
                    return Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (data.currency_type === 1) {
                        return "USD";
                    }
                    else return "IDR";
                    
                    //return "Rp" + Number(data.currency_type).toLocaleString();
                }
            }
        ],

        select: true,
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalFuel").modal({ backdrop: false })
                    $("#modalFuel input[name='action']").val("create");
                }
            },
            //{
            //    text: "Edit",
            //    action: function () {
            //        var a = fuelTable.rows('.selected').indexes();
            //        var b = fuelTable.row(a).data();
            //        if (a.length !== 0) {
            //            $("#modalFuel").modal({ backdrop: false })
            //            $("#modalFuel input[name='tgl']").val(b.tgl);
            //            $("#modalFuel input[name='cost']").val(b.cost_usd);
            //            $("#modalFuel input[name='action']").val("update");
            //            $("#modalFuel input[name='id']").val(b.id);
            //        }
            //    }
            //},
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    CharterEditor = new $.fn.dataTable.Editor({
        ajax: "api/hire",
        table: "#hireTable"
    })

    CharterTable = $("#hireTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/hire",
            type: 'post'
        },
        select: true,
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "tgl_start" },
            { data: "tgl_end" },
            { data: "vessel" },
            { data: "cost_usd" },
            { data: "mob_cost" },
            {
                data: null, render: function (data) {
                    if (data === 1)
                    {
                        return "USD"
                    }
                    else return "IDR"
                    
                }
            }
            //,
            //{
            //    data: null, render: function (data) {
            //        return "Rp" + Number(data.cost_rp).toLocaleString();
            //    }
            //}
        ],
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalCharter").modal({ backdrop: false });
                    $("#modalCharter button[type='submit']").text("Add")
                    $("#modalCharter input").val(null);
                    $("#modalCharter input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = CharterTable.rows('.selected').indexes();
                    var b = CharterTable.row(a).data();
                    console.log(b);
                    var aa = $("#curr_cat");
                    //a.options[i].value
                    console.log(aa[0].options.length);
                    if (a.length > 0) {
                        $("#modalCharter").modal({ backdrop: false });
                        $("#modalCharter input[name='tgl_start']").val(b.tgl_start);
                        $("#modalCharter input[name='tgl_end']").val(b.tgl_end);
                        $("#modalCharter input[name='charter_cost']").val(b.cost_usd);
                        $("#modalCharter input[name='mob_cost']").val(b.mob_cost);
                        SetSelectedIndex("#curr_cat", b.curency_cat);

                        //(b.curency_cat === 1) ? $("#modalCharter select[name='currency_cat'] option:selected").text("USD") : $("#modalCharter select[name='currency_cat'] option:selected").text("IDR");
                        //$("#modalCharter select[name='currency_cat'] option:selected").text(b.curency_cat);
                        $("#modalCharter input[name='action']").val("update");
                        $("#modalCharter input[name='id']").val(b.id);
                        $("#modalCharter button[type='submit']").text("Update")
                        $("#modalCharter select[id='vessel'").val(b.vessel)
                    }
                }
            },
            { extend: "remove", editor: CharterEditor }
        ]
    }).on('init', function () {
        i = 1;
    })

    editor = new $.fn.dataTable.Editor({
        ajax: "api/currency",
        table: "#currencyTable"
    })

    currencyTable = $("#currencyTable").DataTable({
        dom: '<B<"floatright"f>>rtip',
        ajax: {
            url: "api/currency",
            type: "post"
        },
        serverSide: true,
        columns: [
            { data: "currency_name" },
            { data: "currency_value" },
            { data: "last_up" }
        ],
        select: true,
        buttons: [
            {
                text: "Edit", action: function (e, dt, node, config) {
                    var q = currencyTable.rows('.selected').indexes()
                    if (q.length !== 0) {
                        var a = currencyTable.row(q).data();
                        $("#modalCurrency").modal({
                            backdrop: false
                        })
                        $("#modalCurrency form input[name='currency_name']").val(a.currency_name);
                        $("#modalCurrency form input[name='currency_value']").val(a.currency_value);
                        $("#modalCurrency form input[name='currency_id']").val(a.id);
                    }
                }
            },
            { extend: "remove", editor: editor }
        ]
    })

    editor = new $.fn.dataTable.Editor({
        ajax: "api/demob",
        table: "#demobTable"
    })

    DemobTable = $("#demobTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/demob",
            type: "post"
        },
        select: true,
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "tgl" },
            { data: "vessel" },
            {
                data: null, render: function (data) {
                    return "$" + Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data) {
                    return "Rp" + Number(data.cost_rp).toLocaleString();
                }
            }
        ],
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalDemob").modal({ backdrop: false });
                    $("#modalDemob button[type='submit']").text("Add")
                    $("#modalDemob input").val(null);
                    $("#modalDemob input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = DemobTable.rows('.selected').indexes();
                    var b = DemobTable.row(a).data();
                    if (a.length > 0) {
                        $("#modalDemob").modal({ backdrop: false });
                        $("#modalDemob input[name='tgl']").val(b.tgl);
                        $("#modalDemob input[name='cost']").val(b.cost_usd);
                        $("#modalDemob input[name='action']").val("update");
                        $("#modalDemob input[name='id']").val(b.id);
                        $("#modalDemob select[id='vessel'").val(b.vessel)
                        $("#modalDemob button[type='submit']").text("Update")
                    }
                }
            },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/users",
        table: "#userTable"
    })

    userTable = $("#userTable").DataTable({
        //dom: "<'top'Bf>rt<'bottom'lp><'clear'>",
        dom : "Brftip",
        ajax: {
            url: "api/users",
            type: "post"
        },
        select: true,
        columns: [
            { data: "username" },
            { data: "tingkat" }
        ],
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalUser").modal({ backdrop: false });
                    $("#modalUser button[type='submit']").text("Add")
                    $("#modalUser input").val(null);
                    $("#modalUser input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = userTable.rows('.selected').indexes();
                    var b = userTable.row(a).data();
                    if (a.length > 0) {
                        $("#modalUser").modal({ backdrop: false });
                        $("#modalUser input[name='action']").val("update");
                        $("#modalUser input[name='username']").val(b.username);
                        $("#modalUser input[name='id']").val(b.username);
                        $("#modalUser input[name='password']").attr("placeholder", "New Password");
                        $("#modalUser select[name='level'] option:selected").text(b.tingkat);
                        $("#modalUser button[type='submit']").text("Update")
                    }
                }
            },
            { extend: "remove", editor: editor }
        ]
    })

    $("#modalBarge form").submit(function (e) {
        var data = $(this).serialize();
        console.log(data);
        
        $.post("api/cs/barge", data, function (res) {
            if (res == "success") {
                $("#modalBarge").modal('hide');
                unitTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })

    $("#modalCharter form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/charter", data, function (res) {
            if (res == "success") {
                $("#modalCharter").modal('hide');
                CharterTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
    $("#modalDemob form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/demob", data, function (res) {
            if (res == "success") {
                $("#modalDemob").modal('hide');
                DemobTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })

    $("#modalCurrency form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/currency", data, function (res) {
            if (res == "success") {
                $("#modalCurrency").modal('hide');
                CharterTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    });
    $('#tbl_cari').click(function () {
        console.log('pencet tombol cari');
        var data = $('#tgl_cari').val();
        console.log($('#tgl_cari').val());
        $.get("api/unit_f", {tg:$('#tgl_cari').val()}, function (data, status, xhr) {
            console.log(data);
            console.log(status);
            
            //if (res == "success") {

            //    //$("#modalCurrency").modal('hide');
            //    //CharterTable.ajax.reload();
            //} else {
            //    alert(res);
            //}
        })
        //e.preventDefault();
    });

    $("#modalUser form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/users", data, function (res) {
            if (res == "success") {
                $("#modalUser").modal('hide');
                $("#modalUser input").val(null);
                userTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })

    $("#modalBarge form input[name='tgl_from']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalBarge form input[name='tgl_to']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalFuel form input[name='tgl_from']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalFuel form input[name='tgl_to']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#tgl_cari").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalCharter form input[name='tgl_start']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalCharter form input[name='tgl_end']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalDemob form input[name='tgl']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalFuel form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/fuel", data, function (res) {
            if (res == "success") {
                $("#modalFuel").modal('hide');
                fuelTable.ajax.reload();
                $("#form_fuel")[0].reset();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
});