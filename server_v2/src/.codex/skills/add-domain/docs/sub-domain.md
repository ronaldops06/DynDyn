
```
public override async Task<{{Domain}}Entity> UpdateAsync({{Domain}}Entity item)
{
    try
    {
        var {{alias}} = await _context.{{Domain}}
            .Include(t => t.{{Subdomain}})
            .SingleOrDefaultAsync(t => t.Id == item.Id);

        if ({{alias}} == null)
            throw new Exception("No data found");
                
        item.DataCriacao = item.DataCriacao ?? {{alias}}.DataCriacao;
        item.DataAlteracao = DateTime.Now;

        _context.Entry({{alias}}).CurrentValues.SetValues(item);

        var newIds = item.{{FieldListSubDomain}}?.Select(i => i.{{subDomainKey}}).ToHashSet();

        {{alias}}.{{Subdomain}}?.RemoveAll((i => !newIds.Contains(i.{{subDomainKey}})));
                
        var existIds = {{alias}}.{{SubDomain}}?.Select(i => i.{{subDomainKey}}).ToHashSet();

        {{alias}}.{{SubDomain}}?.AddRange(
                    item.{{FieldListSubDomain}}?.FindAll(i => !existIds.Contains(i.{{subDomainKey}})));

        await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        throw ex;
    }

    return item;
}
```