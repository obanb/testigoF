namespace helloworld.Issue.Utils

module convertors =
    open helloworld.Issue.Types.domain
    open Newtonsoft.Json
    open Suave

    let userRequestToUser (issue: CreateIssueInput): Issue =
        { name = issue.name
          content = issue.content
          desc = issue.desc
          Id = "" }

    let deserializeRequest request =
        JsonConvert.DeserializeObject<CreateIssueInput>
            (request.rawForm
             |> System.Text.ASCIIEncoding.UTF8.GetString)
