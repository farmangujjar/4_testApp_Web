var app = angular.module('shp_pick', []);

function txtQtyInCase_change() {

    var txtQtyInCase = document.getElementById("txtQtyInCase").value;

    if (parseInt(txtQtyInCase) <= 0) {


        txtQtyInCase = 1;

        $("#txtQtyInCase").val(txtQtyInCase);
    }

    var skuLabelNeeds = document.getElementById("hfSkuLabelNeeds").value;
    var skuLabelPrinter = document.getElementById("hfSkuLabelPrinter").value;


    var itmLblsPrinted = (parseInt(skuLabelNeeds) + parseInt(skuLabelPrinter));
    var tos = Math.floor(parseInt(itmLblsPrinted) / parseInt(txtQtyInCase));
    var partCase = itmLblsPrinted - (tos * txtQtyInCase);

   
    $("#txtBoxLabels").val(tos + ((partCase > 0) ? 1 : 0));
    $("#lblPartialCase").val("1 Partial Case" + "<br/>" + "Qty: " + partCase);

}


app.controller("shp_pick_controller", function ($scope, $http) {
    $scope.txtval = document.getElementById("txtQtyInCase").value;

    $scope.ResetAll = function () {
        
        var txtQtyInCase = document.getElementById("txtQtyInCase").value;

        if (parseInt(txtQtyInCase) <= 0) {
            txtQtyInCase = 1;
            $("#txtQtyInCase").val(txtQtyInCase);
        }

        var skuLabelNeeds = document.getElementById("hfSkuLabelNeeds").value;
        var skuLabelPrinter = document.getElementById("hfSkuLabelPrinter").value;


        var itmLblsPrinted = (parseInt(skuLabelNeeds) + parseInt(skuLabelPrinter));
        var tos = Math.floor(parseInt(itmLblsPrinted) / parseInt(txtQtyInCase));
        var partCase = itmLblsPrinted - (tos * txtQtyInCase);
        
        $("#txtBoxLabels").val(tos + ((partCase > 0) ? 1 : 0));
        $("#lblPartialCase1").html("1 Partial Case");
        $("#lblPartialCase2").html("Qty: " + partCase);
    };
    $scope.ResetAll();

    $scope.stp_Shipment_Items = [];

    function LoadItems() {

        var shipmentID = document.getElementById("hfShipmentID").value;
        var sku = document.getElementById("hfSku").value;
        var whseUser = document.getElementById("hfWhseUser").value;
        var shipDetailID = document.getElementById("hfShipDetailID").value;
        var qtyInCase = document.getElementById("hfQtyInCase").value;
        var palletNo = document.getElementById("hfPalletNo").value;
        var expDate = document.getElementById("hfExpDate").value;

        
        //alert(shipmentID + " , " + sku + " , " + whseUser + " , " + shipDetailID + " , " + qtyInCase + " , " + qtyInCase + " , " + palletNo + " , " + expDate);

        $http({
            method: 'GET',
            url: '/FBAShipments_Pick/GetItems',
            params: { 'shipmentID': shipmentID, 'sku': sku, 'whseUser': whseUser, 'shipDetailID': shipDetailID, 'QtyInCase': qtyInCase, 'palletNo': palletNo, 'expDate': expDate }

            
        }).then(function (response) {

            angular.forEach(response.data.List, function (value) {
                $scope.stp_Shipment_Items.push(value);
            });

            $scope.change_item_selection($scope.stp_Shipment_Items[0]);

        }, function () {
            $scope.IsLoading = false;
        });
    }
    LoadItems();
    

    var maxItmQty = 0;
    $scope.change_item_selection = function (row) {
        $(".selected_itm").removeClass("selected_itm");

        var _Item = row.ItemNum;
        var _uom = row.UOM;
        var _qtyToPick = row.qtyTake;

        maxItmQty = _qtyToPick;
        $scope.mQtyPicked = 0;


        $("#txtUomQty").html(_uom + " = " + _qtyToPick);
        var id = "tr_itm_" + _uom;
        $("#" + id + "").addClass("selected_itm");
    };
    $scope.change_itm_qty = function (action) {
        var curQty = $scope.mQtyPicked;
        

        if (action === "x") {
            curQty = 0;
        }
        else if (action === "<" && curQty > 0) {
            curQty = curQty.slice(0, -1);
        }
        else if (action === "<" && curQty === 0) {
            curQty = 0;
        }
        else if (action === "+") {
            if (curQty === "") {
                curQty = 1;
            }
            else {
                curQty = parseInt(curQty) + 1;
            }
        }
        else if (action === "-") {
            if (curQty <= 0) {
                curQty = 0;
                return;
            }
            curQty = parseInt(curQty) - 1;
        }
        else {
            if (curQty === 0 || curQty === "0") {
                curQty = "";
            }
            curQty = curQty + "" + action;
        }

        if (curQty > maxItmQty) {
            return;
            //curQty = maxItmQty;
        }

        if (curQty === "" || curQty === "0" ) {
            curQty = 0;
        }
        $scope.mQtyPicked = curQty;
        //$("#txtQtyPicked").val(curQty);
    };

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    };

    //$('#tableCustomer').dataTable();

    //$scope.anyFunc = function (val) {
    //    $scope.Update_FbaLabelPrints(val);
    //};
    
    

    //$scope.Update_FbaLabelPrints = function (palletnumbers) {

    //    alert(palletnumbers);

    //    $http({
    //        method: 'GET',
    //        url: '/FBAShipments_Pick/Update_Pallets',
    //        params: { 'palletnumbers': palletnumbers }
    //    }).then(function (response) {

    //        angular.forEach(response.data.List, function (value) {
    //            $scope.stp_Shipments_GetItemsInSku_Result.push(value);
    //        });

    //        $("#shpDetal2").removeAttr("style");
    //    }, function () {
    //        $scope.IsLoading = false;
    //    });
    //};
    $scope.HasPassport = true;
    $scope.EnableDisable = function () {
        //If TextBox is disabled it will be enabled and vice versa.
        $scope.IsDisabled = !$scope.HasPassport;
    };


    var selectPrepType = document.getElementById("hfSelectPrepType").value;
    $scope.selectedPrepType = selectPrepType;
    $scope.PrepSizes = [];


    $scope.cmbPrepType_change = function () {
        $scope.PrepSizes = [];
        if ($scope.selectedPrepType !== 0) {
            var response = get_choice_dropdown('Prepping', $scope.selectedPrepType);
        }
    };

    function get_choice_dropdown(prepType, description) {
        $http({
            method: 'GET',
            url: '/FBAShipments_Pick/GetChoiceDropDown',
            params: { 'prepType': prepType, 'description': description }
        }).then(function (response) {
            
            angular.forEach(response.data.List, function (value) {
                if (value !== null) {
                    $scope.PrepSizes.push(value);
                }
            });

            //$scope.selectedPrepSize = $scope.PrepSizes[1];

            }, function () {
                return false;
        });
    }

    $scope.EnableDisable();
    $scope.cmbPrepType_change();
});