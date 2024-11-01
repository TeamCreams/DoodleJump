## 사용법
1. 도구 - 패키지 관리자 콘솔 - 기본 프로젝트 해당프로젝트로 설정
2. 
```
add-migration "MigrationName"
혹은 외부에서 PowerShell로
 dotnet ef migrations add "migrationName"

```
[MigrationName] 이름으로 DB 스키마를 형상기억하도록 함.

3.
```
update-database
혹은 외부에서
dotnet ef database update
```

혹시나 잘못 적용하였다면
```
remove-migration
혹은 외부에서
dotnet ef migrations remove
```


