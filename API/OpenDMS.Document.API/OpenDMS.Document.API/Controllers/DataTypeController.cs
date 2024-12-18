using Microsoft.AspNetCore.Mvc;
using OpenDMS.Domain.Models;
using OpenDMS.Core.BusinessLogic;
using OpenDMS.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using OpenDMS.Core.Interfaces;
using OpenDMS.Domain.Services;
using OpenDMS.Domain.Entities;
using System.Collections.Generic;

namespace OpenDMS.DocumentManager.Controllers;

[Authorize]
[ApiController]
[Route("api/document/[controller]")]
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
    [HttpGet("Search/{FieldType}/{MaxResults}/{SearchText}")]
    public async Task<ActionResult<List<FieldTypeValue>>> Search(string FieldType, int MaxResults, string SearchText)
    {
        var M = await customFieldService.GetById(FieldType);
        var datamanger = await factory.Instance(M.DataType);
        if (MaxResults == 0) MaxResults = 8;
        if (MaxResults > 1000) MaxResults = 1000;
        return await datamanger.Search(M, SearchText, MaxResults);
    }

    [HttpGet("Lookup/{FieldType}/{Value}")]
    public async Task<ActionResult<FieldTypeValue>> Lookup(string FieldType, string Value)
    {
        var M = await customFieldService.GetById(FieldType);
        var datamanger = await factory.Instance(M.DataType);
        return await datamanger.Lookup(M, Value);
    }







}