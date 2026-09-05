import { StyleSheet } from "react-native";

export const getIncidenceStyle = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    paddingHorizontal: 15,
    paddingVertical: 10,
    backgroundColor: theme.colors.secondaryBaseColor,
  },

  transactionCard: {
    backgroundColor: theme.colors.secondaryBaseColor,
    borderRadius: 12,
    borderWidth: 1,
    borderColor: theme.colors.primaryBorderColor,
    padding: 15,
    marginBottom: 20,
  },
  cardHeader: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  cardIconContainer: {
    width: 50,
    height: 50,
    borderRadius: 8,
    backgroundColor: theme.colors.primaryBaseColor,
    justifyContent: "center",
    alignItems: "center",
    marginRight: 12,
  },
  cardIcon: {
    fontSize: 24,
  },
  cardContent: {
    flex: 1,
    justifyContent: "center",
  },
  cardTitle: {
    fontSize: 14,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 4,
  },
  cardDate: {
    fontSize: 12,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
  },
  cardValue: {
    fontSize: 16,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginLeft: 12,
  },

  reuseSection: {
    backgroundColor: theme.colors.primaryBaseColor,
    borderRadius: 12,
    padding: 15,
    marginBottom: 15,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  reuseContent: {
    flex: 1,
    marginRight: 10,
  },
  reuseTitle: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 4,
  },
  reuseSubtext: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
  },
  reuseButton: {
    backgroundColor: theme.colors.secondaryBaseColor,
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 8,
  },
  reuseButtonText: {
    fontSize: 12,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryBaseColor,
  },

  otherTransactionsLink: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingVertical: 12,
    marginBottom: 20,
    paddingHorizontal: 5,
  },
  otherTransactionsText: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryBaseColor,
    textDecorationLine: "underline",
  },

  section: {
    marginBottom: 20,
  },
  sectionHeader: {
    marginBottom: 15,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "flex-start",
  },
  sectionTitle: {
    fontSize: 14,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 4,
  },
  sectionSubtitle: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
    maxWidth: "90%",
  },

  addButton: {
    borderWidth: 1,
    borderColor: theme.colors.primaryBaseColor,
    borderRadius: 8,
    paddingVertical: 12,
    paddingHorizontal: 15,
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center",
    marginBottom: 12,
    backgroundColor: theme.colors.secondaryBaseColor,
  },
  addButtonText: {
    fontSize: 12,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryBaseColor,
    marginLeft: 8,
  },

  itemsList: {
    maxHeight: 200,
    marginBottom: 10,
  },

  abatimentoCard: {
    backgroundColor: theme.colors.secondaryBaseColor,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: theme.colors.primaryBorderColor,
    padding: 12,
    marginBottom: 10,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  abatimentoInfo: {
    flex: 1,
  },
  abatimentoMain: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 4,
  },
  abatimentoSecondary: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
  },
  abatimentoRight: {
    alignItems: "flex-end",
  },
  abatimentoValue: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginTop: 6,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 4,
    marginBottom: 6,
  },
  statusText: {
    fontSize: 10,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.secondaryBaseColor,
  },

  resumoSection: {
    backgroundColor: theme.colors.primaryBorderColor,
    borderRadius: 12,
    padding: 15,
    marginBottom: 20,
  },
  resumoTitle: {
    fontSize: 14,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 4,
  },
  resumoSubtitle: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
    marginBottom: 12,
  },
  resumoContent: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingVertical: 10,
  },
  resumoItem: {
    flex: 1,
    alignItems: "center",
  },
  resumoLabel: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
    marginBottom: 4,
  },
  resumoValue: {
    fontSize: 14,
    fontFamily: "Open Sans",
    fontWeight: "600",
  },
  resumoEquals: {
    fontSize: 16,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
    marginHorizontal: 8,
  },
  resumoStatus: {
    marginLeft: 12,
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 4,
    justifyContent: "center",
    alignItems: "center",
  },
  resumoStatusText: {
    fontSize: 11,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.secondaryBaseColor,
  },

  impactoCard: {
    backgroundColor: theme.colors.secondaryBaseColor,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: theme.colors.primaryBorderColor,
    padding: 12,
    marginBottom: 10,
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  impactoInfo: {
    flex: 1,
  },
  impactoMain: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
    marginBottom: 6,
  },
  impactoDetails: {
    gap: 2,
  },
  impactoSecondary: {
    fontSize: 11,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
  },
  impactoRight: {
    alignItems: "flex-end",
  },
  impactoValue: {
    fontSize: 12,
    fontFamily: "Open Sans",
    fontWeight: "600",
    marginBottom: 6,
  },
  impactoAmount: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryTextColor,
  },

  emptyText: {
    fontSize: 12,
    fontFamily: "Open Sans",
    color: theme.colors.secondaryTextColor,
    textAlign: "center",
    paddingVertical: 20,
  },

  buttonsContainer: {
    flexDirection: "row",
    gap: 12,
    marginBottom: 30,
    marginTop: 20,
  },
  button: {
    flex: 1,
    paddingVertical: 14,
    borderRadius: 8,
    justifyContent: "center",
    alignItems: "center",
  },
  buttonCancel: {
    borderWidth: 1,
    borderColor: theme.colors.primaryBaseColor,
    backgroundColor: theme.colors.secondaryBaseColor,
  },
  buttonCancelText: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.primaryBaseColor,
  },
  buttonSave: {
    backgroundColor: theme.colors.primaryBaseColor,
  },
  buttonSaveText: {
    fontSize: 13,
    fontFamily: "Open Sans",
    fontWeight: "600",
    color: theme.colors.secondaryBaseColor,
  },
});
