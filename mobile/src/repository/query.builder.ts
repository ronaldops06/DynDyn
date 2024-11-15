type Condition = {
    field: string;
    operator: '=' | '!=' | '>' | '<' | '>=' | '<=';
    value: string | number;
    alias: string | undefined;
};

type Join = {
    table: string;
    field1: string;
    field2: string;
};

class QueryBuilder {
    private table: string;
    private fields: string[] = [];
    private conditions: Condition[] = [];
    private joins: Join[] = [];
    private limitValue?: number;
    private offsetValue?: number;
    private orderByField?: string;
    private orderByDirection: 'ASC' | 'DESC' = 'ASC';
  
    constructor(table: string) {
      this.table = table;
    }
  
    select(...fields: string[]): QueryBuilder {
      this.fields = fields;
      return this;
    }

    innerJoin(table: string, field1: string, field2: string): QueryBuilder {
        this.joins.push({ table, field1, field2 });
        return this;
    }
  
    where(field: string, operator: Condition['operator'], value: string | number, alias?: string | undefined): QueryBuilder {
      this.conditions.push({ field, operator, value, alias });
      return this;
    }
  
    orderBy(field: string, direction: 'ASC' | 'DESC' = 'ASC'): QueryBuilder {
      this.orderByField = field;
      this.orderByDirection = direction;
      return this;
    }
  
    limit(limit: number): QueryBuilder {
      this.limitValue = limit;
      return this;
    }
    
    offset(offset: number): QueryBuilder {
      this.offsetValue = offset;
      return this;
    }
  
    build(): { query: string; params: (string | number)[] } {
      let query = '';
      let params: (string | number)[] = [];

      try{
        const fieldsPart = this.fields.length > 0 ? this.fields.join(', ') : '*';
        query = `SELECT ${fieldsPart} FROM ${this.table}`;

        this.joins.forEach(join => {
          query += ` INNER JOIN ${join.table} ON ${this.table}.${join.field1} = ${join.table}.${join.field2}`;
        });

        if (this.conditions.length > 0) {
          const conditionsPart = this.conditions
            .map(condition => {
              params.push(condition.value);

              return `${condition.field} ${condition.operator} ?`;
            })
            .join(' AND ');
          query += ` WHERE ${conditionsPart}`;
        }

        if (this.orderByField) {
          query += ` ORDER BY ${this.orderByField} ${this.orderByDirection}`;
        }

        if (this.limitValue !== undefined) {
          query += ` LIMIT ${this.limitValue}`;
        }
        
        if (this.offsetValue !== undefined) {
          query += ` OFFSET ${this.offsetValue}`;
        }
      } catch (error) {
        throw error;
      }
      return { query, params };
    }
  }