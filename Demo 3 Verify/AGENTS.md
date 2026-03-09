# SUMMARY
In this demo we have an "old" API that we want to migrate to a new API rebuilt on an new stack. The demo should showcase this by mocking an "old" API and then in parallel also run a new one. The test project uses Verify to get snapshots from both API:s and Verify that they are the same.

The respones should include some response unique data like guids and timestamps to showcase how you can scrub that data with Verify.

## Covered concepts are
* Using Verify snapshot testing to verify that responses from old and new API are the same.

## Documentation
IMPORTANT! Always prefer official documentation when available.
https://github.com/VerifyTests/Verify
https://github.com/VerifyTests/Verify.Http