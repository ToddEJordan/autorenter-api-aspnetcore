# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue, email, or any other method with the owners of this repository before making a change. 

Please note we have a code of conduct; please follow it in all your interactions with the project.

## Creating an Issue

Before you create a new Issue:

* Check the [Issues](https://github.com/fusionalliance/autorenter-api-aspnetcore/issues) on GitHub to ensure one doesn't already exist.
* Clearly describe the issue, including the steps to reproduce the issue.
* If it's a new feature, enhancement, or restructure, explain your reasoning on why your think it should be implemented, and also provide a supporting use case.

## Making changes

* Create a feature branch from the development branch.
* Check for unnecessary whitespace/changes with `git diff --check` before committing.
* Keep git commit messages clear and appropriate.
* Make sure you have added any tests necessary to test your code.
  * Run __all__ the tests to ensure nothing else was accidentally broken.
  * Don't rely on the existing tests to see if you've broken code elsewhere; test the changes you made in a browser too!
* Update the documentation to go along with any changes in functionality/improvements.

## Pull Request (PR) Process

1. Ensure any install or build dependencies that are no longer needed are removed before the end of the build.
1. Update the README.md with details of changes to the interface or its usage. This includes new environment variables, exposed ports, useful file locations, and container parameters.
1. Increase the version numbers in any example files and the README.md to the new version that this PR would represent. The versioning scheme we use is [SemVer](http://semver.org/).
1. PRs must be approved before they can be merged.
  * Merging of approved PRs to the development branch will be performed by the AutoRenter Core team.
  * Merging of approved PRs to a feature branch will typically be performed by the branch's creator.

    > **Example**: Jon creates a PR for a new feature. To fix some problems he found in Jon's branch while reviewing Jon's PR, Dave creates a new branch from Jon's branch and opens a new PR. After getting necessary approval, Dave merges his PR into Jon's branch. Then, after Jon's PR has been approved, a member of AutoRenter Core team merges Jon's PR into the development branch.

## Recommended Development Workflow

The following steps describe the recommended development workflow.

1. Pull from the `development` branch.
1. As described in the [README](README.md):
  1. Build the app to install project libraries.
  1. Run tests.
  1. Start the API.
1. Browse the API.

If you are implementing a new feature, in addition to the previous steps you should:

1. Create a feature branch by branching off of `development`.
1. Implement your feature. *Note that during this process you should regularly (at least 1x/day) merge the `development` branch into your feature branch to ensure your code is staying current with work being done by the rest of the team.*
  1. Develop
    1. Make changes to code, scripts, unit tests, etc.
    1. Run the tests.
    1. Browse the API.
    1. Repeat until you have something meaningful to commit to your feature branch.
  1. Commit changes to your feature branch.
  1. Repeat these feature implementation steps until the feature is ready to review.
1. Open a pull request to merge your feature branch into `development`.

## Contributor Covenant Code of Conduct

### Our Pledge

In the interest of fostering an open and welcoming environment, we as
contributors and maintainers pledge to making participation in our project and
our community a harassment-free experience for everyone, regardless of age, body
size, disability, ethnicity, gender identity and expression, level of experience,
nationality, personal appearance, race, religion, or sexual identity and
orientation.

### Our Standards

Examples of behavior that contributes to creating a positive environment
include:

* Using welcoming and inclusive language
* Being respectful of differing viewpoints and experiences
* Gracefully accepting constructive criticism
* Focusing on what is best for the community
* Showing empathy towards other community members

Examples of unacceptable behavior by participants include:

* The use of sexualized language or imagery and unwelcome sexual attention or
advances
* Trolling, insulting/derogatory comments, and personal or political attacks
* Public or private harassment
* Publishing others' private information, such as a physical or electronic
  address, without explicit permission
* Other conduct which could reasonably be considered inappropriate in a
  professional setting

### Our Responsibilities

Project maintainers are responsible for clarifying the standards of acceptable
behavior and are expected to take appropriate and fair corrective action in
response to any instances of unacceptable behavior.

Project maintainers have the right and responsibility to remove, edit, or
reject comments, commits, code, wiki edits, issues, and other contributions
that are not aligned to this Code of Conduct, or to ban temporarily or
permanently any contributor for other behaviors that they deem inappropriate,
threatening, offensive, or harmful.

### Scope

This Code of Conduct applies both within project spaces and in public spaces
when an individual is representing the project or its community. Examples of
representing a project or community include using an official project e-mail
address, posting via an official social media account, or acting as an appointed
representative at an online or offline event. Representation of a project may be
further defined and clarified by project maintainers.

### Enforcement

Instances of abusive, harassing, or otherwise unacceptable behavior may be
reported by contacting the project team at [code@fusionalliance.com](mailto:code@fusionalliance.com). All
complaints will be reviewed and investigated and will result in a response that
is deemed necessary and appropriate to the circumstances. The project team is
obligated to maintain confidentiality with regard to the reporter of an incident.
Further details of specific enforcement policies may be posted separately.

Project maintainers who do not follow or enforce the Code of Conduct in good
faith may face temporary or permanent repercussions as determined by other
members of the project's leadership.

### Attribution

This Code of Conduct is adapted from the [Contributor Covenant][homepage], version 1.4,
available at [http://contributor-covenant.org/version/1/4][version]

[homepage]: http://contributor-covenant.org
[version]: http://contributor-covenant.org/version/1/4/
