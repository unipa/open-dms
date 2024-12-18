using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Models;
using System.Collections.Generic;

namespace Web.Controllers.Documents;

[Authorize]
[ApiController]
[Route("internalapi/ui/[controller]")]
public class DataTypeController : ControllerBase
{
    private readonly ICustomFieldService customFieldService;
    private readonly IDataTypeFactory factory;
    private readonly ILoggedUserProfile userContext;

    public DataTypeController(
        ICustomFieldService customFieldService,
        IDataTypeFactory factory,
        ILoggedUserProfile userContext
        )
    {
        this.customFieldService = customFieldService;
        this.factory = factory;
        this.userContext = userContext;
    }

    /// <summary>
    /// effettua una ricerca all'interno di un tipo di metadato.
    /// </summary>
    /// <param name="FieldType">Tipo di campo su cui cercare. Alcune tipolgoie sono predefinite.
    /// Ad esempio:
    /// "$us" sono gli utenti
    /// "$gr" sono i gruppi dell'organigramma
    /// "$ro" sono i ruoli degli utenti
    /// "$ug" permette di selezionare uno qualsiasi dei precedenti profili</param>
    /// <param name="MaxResults">numero massimo di risultati (8=default)</param>
    /// <param name="SearchText">testo da cercare</param>
    /// <returns></returns>
    [HttpGet("Search/{FieldType}/{MaxResults}/{SearchText?}")]
    public async Task<ActionResult<List<FieldTypeValue>>> Search(string FieldType, int MaxResults, string? SearchText)
    {
        var M = await customFieldService.GetById(FieldType);
        if (M == null) return null;
        var datamanger = await factory.Instance(M.DataType);
        if (MaxResults == 0) MaxResults = 8;
        if (MaxResults > 1000) MaxResults = 1000;
        return await datamanger.Search(M, SearchText+"", MaxResults);
    }

    /// <summary>
    /// effettua una ricerca all'interno di un tipo di metadato.
    /// </summary>
    /// <param name="FieldType">Tipo di campo su cui cercare. Alcune tipolgoie sono predefinite.
    /// Ad esempio:
    /// "$us" sono gli utenti
    /// "$gr" sono i gruppi dell'organigramma
    /// "$ro" sono i ruoli degli utenti
    /// "$ug" permette di selezionare uno qualsiasi dei precedenti profili</param>
    /// <param name="MaxResults">numero massimo di risultati (8=default)</param>
    /// <param name="Value">testo da cercare</param>
    /// <returns></returns>
    [HttpGet("Lookup/{FieldType}/{MaxResults}/{Value}")]
    public async Task<ActionResult<List<FieldTypeValue>>> Lookup(string FieldType, int MaxResults, string Value)
    {
        var M = await customFieldService.GetById(FieldType);
        var datamanger = await factory.Instance(M.DataType);
        List<FieldTypeValue> Items = new List<FieldTypeValue>();
        foreach (var v in Value.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            Items.Add(await datamanger.Lookup(M, v));
        }
        return Items;
    }







}