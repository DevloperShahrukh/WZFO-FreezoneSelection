function AddToHidden(treeViewName, HiddenValueId) {
    var ParameterIds = '0';
    $('td .' + treeViewName + 'Child').each(function (index) {
        if ($(this).prev().is(':checked')) {

            if (ParameterIds == '0') {
                ParameterIds = $(this).find('span').attr('id');
            }
            else {
                ParameterIds += ',' + $(this).find('span').attr('id');
            }
        }
    });

    $('#' + HiddenValueId).val(ParameterIds);
}

function validateTreeviewNodesSelection(TreeviewID) {
    return ($('td .' + TreeviewID + 'Child').prev('input:checked').length > 0)

}


function UpdateCategoryAnalytics(SubCategoryIds, ModuleName) {

    $.ajax({
        method: "POST",
        url: "/_layouts/15/WFZO.FZSelector/WebMethods.aspx/UpdateCategoryAnalytics",
        data: JSON.stringify({ SubCategoryIds: SubCategoryIds, ModuleName: ModuleName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (msg) {
        },
    });
}



function UpdateFreeZoneAnalytics(ObjectArray) {

    $.ajax({
        method: "POST",
        url: "/_layouts/15/WFZO.FZSelector/WebMethods.aspx/UpdateFreeZoneAnalytics",
        data: JSON.stringify({ DataArray: ObjectArray }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (msg) {
        },
    });
}

function validateReportType(RadioButtonListClienId) {
    var selectedReportType = $('#' + RadioButtonListClienId).find('input:checked');

    if (selectedReportType.length <= 0) { ErrorVariable.Error += ' Select one of the report type \n'; }

    return (selectedReportType.length > 0);
}


// *** Weighted Benchmarking Validation
function validateCheckedInputs(ErrorVariable) {
    var checkedCategories = $("[name*='chkSelectedCategory']:checked");

    if (checkedCategories.length <= 0) { ErrorVariable.Error += ' One of the category must be checked \n'; }

    return (checkedCategories.length > 0);
}

function validateCheckedInputsSum(ErrorVariable) {
    var Sum = 0;
    $("[name*='chkSelectedCategory']:checked").parent().parent().each(function (index) {

        Sum += parseInt($(this).find("[name*='quantity']").val());

    });

    if (Sum != 100) { ErrorVariable.Error += ' The over all weightage should be equal to 100'; }


    return (Sum == 100);
}


// End of Weighted Benchmarking Validation

// *** Weighted Benchmarking Category Analytics Data preparation and update

function UpdateCategoryAnalyticsOfWeigthted(ModuleName) {

    var SubCategoryWithWeights = new Array();

    $("[name*='chkSelectedCategory']:checked").parent().parent().each(function (index) {

        var SubCategoryWithWeight = new Object();
        SubCategoryWithWeight.SubCatIds = $(this).find("[name*='hdnSubCatIds']").val();
        SubCategoryWithWeight.Weight = $(this).find("[name*='quantity']").val();

        SubCategoryWithWeights.push(SubCategoryWithWeight);

    });


    $.ajax({
        method: "POST",
        url: "/_layouts/15/WFZO.FZSelector/WebMethods.aspx/UpdateCategoryAnalyticsOfWeigthted",
        data: JSON.stringify({ SubCategoryIdsWithWeight: SubCategoryWithWeights, ModuleName: ModuleName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (msg) {
            console.log(msg.d);
        },
    });

}

// End of Weighted Benchmarking Validation



function OnTreeClick(evt) {
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        CheckUncheckParents(src, src.checked);
    }
}


function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}


function CheckUncheckParents(srcChild, check) {
    var parentDiv = GetParentByTagName("div", srcChild);
    var parentNodeTable = parentDiv.previousSibling;


    if (parentNodeTable) {
        var checkUncheckSwitch;


        if (check) //checkbox checked
        {
            var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
            if (isAllSiblingsChecked)
                checkUncheckSwitch = true;
            else
                return; //do not need to check parent if any child is not checked
        }
        else //checkbox unchecked
        {
            checkUncheckSwitch = false;
        }


        var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
        if (inpElemsInParentTable.length > 0) {
            var parentNodeChkBox = inpElemsInParentTable[0];
            parentNodeChkBox.checked = checkUncheckSwitch;
            //do the same recursively
            CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
        }
    }
}


function AreAllSiblingsChecked(chkBox) {
    var parentDiv = GetParentByTagName("div", chkBox);
    var childCount = parentDiv.childNodes.length;
    for (var i = 0; i < childCount; i++) {
        if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
        {
            if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                //if any of sibling nodes are not checked, return false
                if (!prevChkBox.checked) {
                    return false;
                }
            }
        }
    }
    return true;
}


//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}