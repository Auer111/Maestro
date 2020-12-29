class Maestro
{
    constructor()
    {
        window.maestro = this;

        this.Request = {};
        this.Data = {};
        this.Chain = {};

        this.VarType =
            [
            {
                color: "#7eb8d9",
                icon: "i-edit",
                tooltip: "Default"
            },
            {
                color: "lightsalmon",
                icon: "i-run",
                tooltip: "Dynamic"
            },
            {
                color: "lightgreen",
                icon: "i-tree",
                tooltip: "Environment"
            },
            {
                color: "yellow",
                icon: "i-globe",
                tooltip: "Global"
            }
        ];
    }

    Overlay(show) { show ? this.overlay.show() : this.overlay.hide(); }

    getPropertyVariable(prop) {
        if (maestro.Request.Variables == null) { return null; }
        return maestro.Request.Variables.hasOwnProperty(prop) ? maestro.Request.Variables[prop] : null;
    }

    CreateTooltips()
    {
        $('[tooltip]').each
            (function ()
            {
                let this$ = $(this);
                let ttText = this$.attr("tooltip");
                this$.addClass("m-tooltip");
                this$.append(`<div class="tooltiptext">${ttText}</div>`);
                this$.removeAttr("tooltip");
            });
    }

    DisplayVariables(el$)
    {
        let content = '';

        el$.children().eq(0).empty();
        el$.children().eq(1).empty();

        for (let prop in maestro.Request.Variables)
        {
            let v = maestro.Request.Variables[prop];

            if (v.Direction == 0) //Request
            {
                content = `
                <div class="py-2 col-12 d-flex">
                    <div class="mt-1 ${maestro.VarType[v.Type].icon}"></div>
                    <span class="my-auto mx-3">${v.Mapping} -> ${prop}</span>
                </div>`
            }
            else  //Response
            {
                content = `
                <div class="py-2 col-12 d-flex">
                    <div class="mt-1 ${maestro.VarType[v.Type].icon}"></div>
                    <span class="my-auto mx-3"> ${prop} -> ${v.Mapping}</span>
                </div>`
                
            }
            el$.children().eq(v.Direction).append(content);
        };
    }
}


class MaestroXml
{
    constructor()
    {
        window.xml = this;
        xml._loaded();

        this.idGen = 0;
    }

    _loaded()
    {

               
    }

    ShowOverlay(button, tagname)
    {
        maestro.Overlay(true);

        let overlay$ = $(button).parents(".xmlDisplay").first().siblings('.AddVariableOverlay').first();

        overlay$.css("top", (button.offsetTop - 19) + "px").css("display", "flex");

        //get overlay fields
        let varmapping$ = overlay$.find(".var-mapping");
        let vartype$ = overlay$.find(".var-type");
        let vartagname$ = overlay$.find(".var-tagname")

        //populate overlay
        vartagname$.val(tagname);
        varmapping$.val("");
        vartype$.val(0);

        //populate overlay if data exists
        let VariableDataObj = maestro.getPropertyVariable(tagname);
        if (VariableDataObj != null)
        {
            varmapping$.val(VariableDataObj.Mapping);
            vartype$.val(VariableDataObj.Type);
        }
        
        overlay$.find(".var-add").one
        ("click",(event) =>
        {
            let newVar = {};

            if (maestro.Request.Variables[tagname] != null && vartype$.val() == '0') { delete maestro.Request.Variables[tagname]; }
            else if (varmapping$.val() == null || varmapping$.val().length < 3) { }
            else
            {
                 newVar =
                 {
                    "Mapping": varmapping$.val(),
                    "Direction": overlay$.attr("direction"),
                    "Type": vartype$.val()
                 }
                 maestro.Request.Variables[tagname] = newVar;
            }
            maestro.Overlay(false);
            overlay$.hide();

            console.log(maestro.Request.Variables);

            RenderRequest(maestro.Request);
        });
    }

    Editor(xmlNode, rootEl$, depth)
    {
        let childNodes = xmlNode.children;
        if (childNodes == null) { return; }
        depth = depth == null ? 0 : depth + 1;

        for (let childNode of childNodes)
        {
            let newRootEl$ = this._createTreeHtml(childNode, rootEl$, depth);
            this.Editor(childNode, newRootEl$, depth);
        }
    }

    _createTreeHtml(DocNode, rootEl$, depth)
    {
        this.idGen++;
        let newID = "docTree" + this.idGen;

        let content;
        let style;
        let el$;

        switch (DocNode.children.length > 0)
        {
            case true: //Is a container
                content = '<br />';
                style = `padding-left: ${depth * 10}px;`

                this.addStyle(`#${newID}::before{ content: '<${DocNode.tagName}>';} 
                               #${newID}::after{ content: '</${DocNode.tagName}>'; position: relative; left: ${depth * 10}px;}`)

                el$ = $(`<${DocNode.tagName} id="${newID}" class="xmlRow" style="${style}">${content}</ ${DocNode.tagName}>`);

                break;

            case false: //Has data

                let variableData = maestro.getPropertyVariable(DocNode.tagName);

                let isDefault = "default";
                let iconClass = "i-edit";
                let buttonColor = "";
                let tooltipText = "Default";
                if (variableData != null)
                {
                    
                    if (variableData.Direction == 0) { isDefault = ""; }
                    buttonColor = maestro.VarType[variableData.Type].color;
                    iconClass = maestro.VarType[variableData.Type].icon;
                    tooltipText = variableData.Mapping;
                }
                

                content = `<button id="${DocNode.tagName}" type="button" onclick="xml.ShowOverlay(this,'${DocNode.tagName}');" class="xmlStaticContent" style="background-color:${buttonColor};"><div class="xmlEditButton ${iconClass} mb-1"></div>${DocNode.tagName}</button>`;
                style = `padding-left: ${depth * 10}px;`

                this.addStyle(`#${newID}::before{ content: '<';} 
                               #${newID}::after{ content: '</${DocNode.tagName}>';}`)
                el$ = $(`<${DocNode.tagName} id="${newID}" class="xmlRow" style="${style}">${content}><div style="display: inline; cursor: help;" tooltip="${tooltipText}" class="xmlContent ${isDefault}">${DocNode.textContent}</div></ ${DocNode.tagName}>`);

                break;
        }

        rootEl$.append(el$);
        rootEl$.append('<br />');

        return el$;
    }

    addStyle(rule) { $(`<style type='text/css'> ${rule} </style>`).appendTo("head"); }
}


new Maestro();

