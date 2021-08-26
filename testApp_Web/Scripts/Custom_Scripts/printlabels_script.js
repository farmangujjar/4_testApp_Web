var app = angular.module('printlabels_app', []);

app.controller("Shipments_PrintLabels_Skus_controller", function ($scope, $http) {

    $scope.loadingShpDetail = false;


    var selectedShipmentID = "";
    var selectedAmzShipmentID = "";
    var selectedSku = "";


    var cellVal = "";
    var shipmentID = "";
    var sku = "";

    $(function () {
        ContextMenuOnRightClick("#shipments_contextMenu", "ul#tvShipments li");
        ContextMenuOnRightClick("#shp_details_contextMenu", "#tblShipmentDetails tr td");
    });

    $scope.stp_Shipments_GetWorkingShipments_Result = [];

    function LoadShipments() {
        $http({
            method: 'GET',
            url: '/PrintLabels/LoadShipments',
            params: {}
        }).then(function (response) {

            angular.forEach(response.data.List, function (value) {
                $scope.stp_Shipments_GetWorkingShipments_Result.push(value);
            });


            var shipmentID = $scope.stp_Shipments_GetWorkingShipments_Result[0].ShipmentID;
            var shipmentName = $scope.stp_Shipments_GetWorkingShipments_Result[0].ShipmentName;

            selectedShipmentID = shipmentID;
            selectedAmzShipmentID = $scope.stp_Shipments_GetWorkingShipments_Result[0].AmzShipmentID;

            var searchStr = '';

            $scope.Get_ShipmentSkus(shipmentID, shipmentName, searchStr);

            $scope.loadingTvShipment = false;
            $("#tvShipments").show("fast");

        }, function () {
            $scope.IsLoading = false;
        });
    }

    $scope.Get_ShipmentSkus = function (shipmentID, shipmentName, searchStr) {

        $("#vwSkuDetails").hide();
        $('#vwShipmentDetails').hide();
        $scope.loadingShpDetail = true;
        


        $('.cls_shipment_detail').css('display', 'none');
        $('.loader_shp_details').css('display', 'inline-block');


        var id = "cls_" + shipmentID;

        $("#shipmentName").html(shipmentName.toString());
        $scope.stp_Shipments_PrintLabels_Skus_Result = [];

        $http({
            method: 'GET',
            url: '/PrintLabels/Get_ShipmentSkus',
            params: { 'shipmentID': shipmentID, 'searchStr': searchStr }
        }).then(function (response) {


            
            var count = response.data.List.length;

            angular.forEach(response.data.List, function (value) {
                count--;


                $scope.stp_Shipments_PrintLabels_Skus_Result.push(value);


                if (count === 0) {
                    setTimeout(function () {
                        $(".p").removeClass("active");
                        $("#" + id + "").addClass("active");


                        $scope.loadingShpDetail = false;
                        $('#vwShipmentDetails').show("slow");
                        

                        var rowData = $scope.stp_Shipments_PrintLabels_Skus_Result;                        
                        $scope.ShipmentSkusDetails(rowData);


                    }, 1);
                }
                else {
                    //alert(count);
                }
            });

        }, function () {
            $scope.IsLoading = false;
        });
    };
    $scope.Get_Shipment_Detail = function (shipmentDetailID) {
        $scope.loadingSkuDetaill = true;
        $("#vwSkuDetails").css('display', 'none');   
        $("#loadingSkuDetail").removeAttr("style");

        var id = "tr_sku_" + shipmentDetailID;
        //$(".selected_sku").removeClass("active");

        $("table#tblShipmentDetails tbody tr").removeClass("selected_sku");
        $("table#tblShipmentDetails tbody tr").removeClass("active");

        $("#" + id + "").addClass("selected_sku");
        

        $scope.stp_Shipments_GetItemsInSku_Result = [];

        $http({
            method: 'GET',
            url: '/PrintLabels/Get_Shipment_Detail',
            params: { 'shipmentDetailID': shipmentDetailID }
        }).then(function (response) {

            angular.forEach(response.data.List, function (value) {
                $scope.stp_Shipments_GetItemsInSku_Result.push(value);
            });

            setTimeout(
                function () {
                    $scope.loadingSkuDetaill = false;
                    $("#loadingSkuDetail").css('display', 'none');            
                    $("#vwSkuDetails").show();
                }, 600);

        }, function () {
            $scope.IsLoading = false;
        });
    };

    LoadShipments();






    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    };


    $scope.greaterThan = function () {
        //alert(stp_Shipments_PrintLabels_Skus_Result.length);
        return function (item) {
            var val = Math.round(item[prop] * 100);
            return val < 1;
        };
    };


    
    $scope.ShipmentSkusDetails = function (row) {

        var rowData = row[0];
        var sku = rowData.Sku;
        var imgItem = rowData.MediumImage;


        $scope.Get_Shipment_Detail(rowData.ShipmentDetailID);
        switch_image(rowData.MediumImage, rowData.MediumImage);
        document.getElementById("imgItem_popup").src = rowData.MediumImage;

        selectedSku = rowData;
    };

    $('#exampleModal').on('shown.bs.modal', function () {
        $('#ddlUser').focus();
    });

    $scope.sku_complete_popup = function () {
        var selectedUser = $('#ddlUser').find(":selected").text();
        if (selectedUser === "Please Select") {
            alert("Please Select User!");
            $('#ddlUser').focus();
            return;
        }

        var qtyInCase = selectedSku.FBA_Per_Case;
        var splitIndex = selectedSku.SplitIndex;
        var qtyCF = selectedSku.FullyPicked;
        var lblsCF = selectedSku.lblsComplete;
        var fbaLblsPrinted = selectedSku.FBALabelsPrinted;
        
        var ex = selectedSku.ExpDate;

        var boxLabels = selectedSku.BoxContentLablelsNeed;

        var fnskuLablelsNeed = selectedSku.FnskuLablelsNeed;
        var fnskuLabelsPrinted = selectedSku.FnskuLabelsPrinted;

        
        var shipmentDetailID = selectedSku.ShipmentDetailID;
        
        var url = 
            "/FBAShipments_Pick?"
            + "shipmentID=" + selectedShipmentID
            + "&"+ "AmzShipmentID='" + selectedAmzShipmentID
            + "'&"+ "sku=" + selectedSku.Sku
            + "&"+ "whseUser='" + selectedUser
            + "'&"+ "shipDetailID=" + shipmentDetailID
            + "&"+ "QtyInCase=" + qtyInCase
            + "&"+ "expDate=" + ex
            + "&"+ "split=" + 0
            + "&"+ "qty=" + qtyCF
            + "&"+ "lbls=" + lblsCF
            + "&"+ "boxLblsPrinted=" + fbaLblsPrinted
            + "&" + "fnskuLabelsPrinted=" + fnskuLabelsPrinted
            + "&" + "boxLabels=" + boxLabels
            + "&" + "fnskuLablelsNeed=" + fnskuLablelsNeed
            + "";

        $('#ddlUser').val('0');

        $("#exampleModal").modal('hide');
        $("#lblSku_Review").html(selectedSku.Sku);
        
        $("#reviewModel").modal('show');

        var $iframe = $("#rvModel");
        if ($iframe.length) {
            $iframe.attr('src', url);
        }
        
        
        //Get_PalletByShipment(selectedShipmentID);
        
    };
    









    

    function ContextMenuOnRightClick(menuID, tableID) {
        //alert(menuID + " - " + tableID);
        var $contextMenu = $(menuID);
        $("body").on("contextmenu", tableID, function (e) {
            shipmentID = $(this).closest("tr").children("td").eq(1).html();
            sku = $(this).closest("tr").children("td").eq(2).html();
            cellVal = $(this).html();

            $contextMenu.css({
                display: "block",
                left: e.pageX,
                top: e.pageY
            });
            return false;
        });


        $('html').click(function () {
            $contextMenu.hide();
        });


        $(menuID + " li#copy").click(function (e) {
            copyToClipboard(cellVal);
            var f = $(this);
        });
        $(menuID + " li#delete").click(function (e) {
            //alert(shipmentID);
            //alert(shipmentID + " - " + sku);
            $("#model_pin_verification").modal('show');
            document.getElementById("txtPin").focus();
        });


        $("#btn_verify_pin").click(function (e) {
            if ($("#txtPin").val()) {

                var pin = $("#txtPin").val();
                alert(shipmentID + " - " + sku);

                $http({
                    method: 'GET',
                    url: '/PrintLabels/Delete_Shipments_By_DeleteSku',
                    params: { 'pin': pin, 'shipmentid': shipmentID, 'sku': sku }
                }).then(function (response) {

                    if (response.data.status === 1) {
                        alert("r");
                    }
                    else {
                        alert("s");
                    }
                    

                }, function () {
                    $scope.IsLoading = false;
                });


            }
            else {
                alert("Empty");
            }
        });


        $(menuID + " li#show_warehouse").click(function (e) {
            alert(shipmentID);
            show_for_warehouse(shipmentID);
        });





        function copyToClipboard(text) {
            var $temp = $("<input>");
            $("body").append($temp);
            $temp.val(text).select();
            document.execCommand("copy");
            $temp.remove();
        }
    }






    function show_for_warehouse(shipmentID) {

        var link = '/PrintLabels/Update_Shipment_Wise';

        var parameters = {
            "shipmentid": shipmentID,
            "showWhse": 1
        };

        ApiCall(link, parameters);

    }


    function ApiCall(link, parameters) {

        $http({
            method: 'GET',
            url: link,
            params: parameters
        }).then(function (response) {

            return response;

        }, function () {
            return false;
        });
    }

    
    $scope.show_completedItems = {};
    $scope.completedItem = function () {        
        return function (skuItems) {
                        
            if ($("#cbCompletedItems").is(":checked")) {
                return true;
            }
            else {

                if ((skuItems.pErcentComplete * 100) < 100) {
                    return true;
                }
            }
        };
    };


    $scope.popup_review = function () {

        
    };


    

});