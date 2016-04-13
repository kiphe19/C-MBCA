
$(document).ready(function () {
    var editorVes = new $.fn.dataTable.Editor({
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
    });

    vesselTable = $('#vesselTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/vessel",
            type: 'post'
        },
        columns: [
            { data: null },
            { data: "name" },
            { data: "vs_desc" }
        ],
        select: true,
        buttons: [
            { extend: 'create', editor: editorVes, text: "Add New Vessel" },
            { extend: 'edit', editor: editorVes },
            { extend: 'remove', editor: editorVes },
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    var today = new Date();
    var yyyy = today.getFullYear();
    var mm = ((today.getMonth()+1)<10)? "0"+(today.getMonth()+1) :(today.getMonth()+1) ;
    var dd  = (today.getDate() <10)? "0"+today.getDate() : today.getDate();
    var hari = yyyy+"-"+mm+"-"+dd; 

    var unitEditor = new $.fn.dataTable.Editor({
        //ajax: "api/unit",
        ajax: "api/unit/"+hari,
        table: "#bargeTable",
        fields: [{
            //label: "First name:",
            name: "unit_table.name"
        }, {
            //label: "Last name:",
            name: "unit_table.ket"
        }, {
            //label: "Manager:",
            name: "unit_distance_table.id",
            type: "select"
        }]
    });
    var unitTable = $("#bargeTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/unit/" + hari,
            type: "post",
        },
        columns: [
            { data: null },
            { data: "unit_table.name" },
            { data: "mainunit_table.nama" },
            { data: "unit_distance_table.distance" },
            { data: "unit_distance_table.tgl" }
        ],
        select: true,
        buttons: [
            {
                text: "New Unit",
                action: function () {
                    $("#modalBarge").modal({ backdrop: false });
                    $("#modalBarge button[type='submit']").text("Add")
                    $("#modalBarge input").val(null);
                    $("#modalBarge input[name='action']").val("create");
                }
            },
            {
                text: "Distance",
                action: function () {
                    $("#modalUnitDistance").modal({ backdrop: false });
                    $("#modalUnitDistance button[type='submit']").text("Add Distance");
                    $("#modalUnitDistance input").val(null);
                }

            },
            {
                text: "Main Unit",
                action: function () {
                    $("#modalUnittoMainUnit").modal({ backdrop: false });
                }
            },
            {
                extend: 'edit',
                text: "Edit Unit",
                action: function () {
                    var a = unitTable.rows('.selected').data(), b = a[0];
                    if (a.length > 0) {
                        $("#modalBarge").modal({ backdrop: false })
                        $("#modalBarge input[name='action']").val("update")
                        $("#modalBarge button[type='submit']").text("Update")
                        $("#modalBarge input[name='unit_name']").val(b.unit_table.name)
                        $("#modalBarge input[name='unit_afe']").val(b.unit_table.afe)
                        $("#modalBarge input[name='id']").val(b.unit_table.id)
                        $("#modalBarge input[name='unit_desc'").val(b.unit_table.ket)
                    }
                }
            },
            { extend: 'remove', text: 'Delete Unit', editor: unitEditor }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    var editorMainunit = new $.fn.dataTable.Editor({
        ajax: "api/mainunit",
        table: "#mainunitTable"
        //fields: [
        //    { label: "Area Name", name: "distance_name" },
        //    { label: "Distance", name: "distance" }
        //]
    })
    var MainunitTable = $('#mainunitTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/mainunit",
            type: "post"
        },
        columns: [
            {data: null},
            { data: "nama" },
            { data: "description" }
        ],
        select: true,
        buttons: [
            {
                extend: "create",
                text: "New Main User",
                action: function () {
                    $("#modalMainUnit").modal({ backdrop: false });
                    $("#modalMainUnit button[type='submit']").text("Add")
                    $("#modalMainUnit input").val(null);
                    $("#modalMainUnit input[name='action']").val("create");
                }
            },
            {
                extend: "edit",
                text: 'Edit',
                action: function () {
                    var a = MainunitTable.rows('.selected').data(), b = a[0];
                    //console.log(b);
                    if (a.length > 0) {
                        $("#modalMainUnit").modal({ backdrop: false });
                        $("#modalMainUnit button[type='submit']").text("Update")
                        $("#modalMainUnit input[name='action']").val("update");
                        $("#modalMainUnit input[name='id']").val(b.id);
                        $("#modalMainUnit input[name='mainunit_name']").val(b.nama);
                        $("#modalMainUnit input[name='mainunit_desc']").val(b.description);
                    }
                }
                //editor: editor
            },
            { extend: "remove", editor: editorMainunit }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }

    });


    var editorDist = new $.fn.dataTable.Editor({
        ajax: "api/distance",
        table: "#distanceTable",
        fields: [
            { label: "Area Name", name: "distance_name" },
            { label: "Distance", name: "distance" }
        ]
    });

    var distanceDT = $("#distanceTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/distance",
            type: 'post'
        },
        columns: [
            {data: null},
            { data: "distance_name" },
            {
                data: null,
                render: function (data, type, row) {
                    return data.distance + " NMi";
                }
            }
        ],
        select: true,
        buttons: [
            { extend: "create", editor: editorDist, text: "Create new Area" },
            { extend: "edit", editor: editorDist },
            { extend: "remove", editor: editorDist }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    var yyyy = today.getFullYear();
    var mm = (today.getMonth() + 1);
    var dd = (today.getDate() < 10) ? "0" + today.getDate() : today.getDate();
    var tg1, tg2;
    if (dd <= 25) {
        tg1 = yyyy + "-" + (((mm - 1) < 10) ? "0" + (mm - 1) : (mm - 1)) + "-" + 25;
        tg2 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
    }
    else {
        tg1 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
        tg2 = yyyy + "-" + (((mm + 1) < 10) ? "0" + (mm + 1) : (mm + 1)) + "-" + 25;
    }
    var Fueleditor = new $.fn.dataTable.Editor({
        ajax: "api/fuel/" + tg1 + "/" + tg2,
        table: "#fuelTable"
    })

    var fuelTable = $("#fuelTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/fuel/"+tg1+"/"+tg2,
            type: "post"
        },
        columns: [
            {data: null},
            { data: "tgl" },
            {
                data: null,
                render: function (data, type, row) {
                    return Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    if (data.currency_type === 1) return "USD";
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
            { extend: "remove", editor: Fueleditor }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    var CharterEditor = new $.fn.dataTable.Editor({
        ajax: "api/charter",
        table: "#hireTable"
    })

    var CharterTable = $("#hireTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/charter",
            type: 'post'
        },
        select: true,
        columns: [
            { data: null },
            { data: "hire_table.tgl_start" },
            { data: "hire_table.tgl_end" },
            { data: "vessel_table.name" },
            { data: "hire_table.cost_usd" },
            { data: "hire_table.mob_cost" },
            {
                data: "hire_table.curency_cat",
                render: function (data) {
                    if (data === 1) return "USD";
                    else return "IDR";
                }
            }
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
                extend: 'edit',
                text: "Edit",
                action: function () {
                    var a = CharterTable.rows('.selected').indexes();
                    var b = CharterTable.row(a).data();
                    if (a.length > 0) {
                        $("#modalCharter").modal({ backdrop: false });
                        $("#modalCharter input[name='tgl_start']").val(b.hire_table.tgl_start);
                        $("#modalCharter input[name='tgl_end']").val(b.hire_table.tgl_end);
                        $("#modalCharter input[name='charter_cost']").val(b.hire_table.cost_usd);
                        $("#modalCharter input[name='mob_cost']").val(b.hire_table.mob_cost);
                        $("#modalCharter select[name = 'currency_cat'] option[value=" + b.hire_table.curency_cat + "]").prop("selected", true);
                        $("#modalCharter select[name = 'vesselid'] option[value=" + b.hire_table.id_vessel + "]").prop("selected", true);
                        $("#modalCharter input[name='action']").val("update");
                        $("#modalCharter input[name='id']").val(b.hire_table.id);
                        $("#modalCharter button[type='submit']").text("Update")
                    }
                }
            },
            { extend: "remove", editor: CharterEditor }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });


    var userEditor = new $.fn.dataTable.Editor({
        ajax: "api/users",
        table: "#userTable"
    })

    var userTable = $("#userTable").DataTable({
        //dom: "<'top'Bf>rt<'bottom'lp><'clear'>",
        dom: "Brftip",
        ajax: {
            url: "api/users",
            type: 'post'
        },
        select: true,
        columns: [
            { data: null },
            { data: "username" },
            {
                data: "tingkat",
                render: function (data) {
                    if (data == "0") return "Administrator";
                    else return "Operator";
                }
            }
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
                        $("#modalUser input[name='id']").val(b.id);
                        $("#modalUser input[name='username']").val(b.username);
                        $("#modalUser input[name='password']").attr("placeholder", "New Password");
                        $("#modalUser select[name='level'] option[value='" + b.tingkat + "']").prop("selected", true);
                        $("#modalUser button[type='submit']").text("Update")
                    }
                }
            },
            { extend: "remove", editor: userEditor }
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    $("#modalBarge form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/unit", data, function (res) {
            if (res == "success") {
                $("#modalBarge").modal('hide');
                unitTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    });
    $("#modalUnitDistance form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/unitdist", data, function (res) {
            if (res == "success") {
                $("#modalUnitDistance").modal('hide');
                unitTable.ajax.url('api/unit/' + hari).load();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    });
    $("#modalUnittoMainUnit form").submit(function (e) {
        var data = $(this).serialize();
        //console.log(data);
        $.post("api/cs/unit_mainunit", data, function (res) {
            if (res == "success") {
                $("#modalUnittoMainUnit").modal('hide');
                $("#modalUnittoMainUnit input").val(null);
                unitTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    });

    $("#modalMainUnit form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/mainunit", data, function (res) {
            if (res == "success") {
                $("#modalMainUnit").modal('hide');
                MainunitTable.ajax.reload();
            } else {
                alert(res);
            }
        });
        e.preventDefault();
    });
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

    $('#tbl_cari').click(function () {
        var sss = new Date($('#tgl_cari').val());
        var carii = sss.getFullYear() + "-" + (sss.getMonth() + 1) + "-" + sss.getDate();
        unitTable.ajax.url('api/unit/' + carii).load();
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
    
    $("#fuelCari input[name='fuel_tg_From']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#fuelCari input[name='fuel_tg_To']").datetimepicker({
        format: "MM/DD/YYYY"
    });

    $("#modalUnitDistance form input[name='tgl_from']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    
    $("#modalUnitDistance form input[name='tgl_to']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalUnittoMainUnit form input[name='tgl_to']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#modalUnittoMainUnit form input[name='tgl_from']").datetimepicker({
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
    });

    $("#tbl_cari_fuel").click(function () {
        var dari = new Date($("#fuelCari input[name = 'fuel_tg_From']").val());
        var ke = new Date($("#fuelCari input[name = 'fuel_tg_To']").val());
        var dari1 = dari.getFullYear() + "-" + (dari.getMonth() + 1) + "-" + dari.getDate();
        var ke1 = ke.getFullYear() + "-" + (ke.getMonth() + 1) + "-" + ke.getDate();
        fuelTable.ajax.url("api/fuel/"+dari1+"/"+ke1).load();
    })
});