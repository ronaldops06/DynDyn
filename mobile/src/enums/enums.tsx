export enum TypesTransaction {    
    Revenue = 1,
    Expense = 2,
    Transference = 3
}

export enum TypesCategory {
    Account = 1,
    Operation = 2
}

export enum TypeTotalizar {
    Ignore = 1,
    Discriminated = 2,
    GroupedRevenue = 3,
    GroupedExpense = 4
}

export enum StatusHttp {
    OK = 200,
    Created = 201,
    Unauthorized = 401,
    BadRequest = 400
}

export enum Action {
    Get = 0,
    Post = 1,
    Put = 2,
    Delete = 3
}

export enum Situation {
    NotConsolidated = 0,
    Consolidated = 1,
    All = -1
}