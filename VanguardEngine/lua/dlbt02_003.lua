-- 共に駆ける光芒 オフィリア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.GetNumberOf("ConditionFilter", {l.PlayerVC}) > 0
end

function ConditionFilter(id)
	return obj.IsAttackingUnit(id) and obj.NameIs(id, obj.GetNameFromCardID("dlbt01_004"))
end

function Activation()
	if obj.GetNumberOf("RubyFilter1", {l.PlayedOrdersThisTurn}) > 0 or obj.GetNumberOf("RubyFilter2", {l.SoulBlasted}) > 0 then
		obj.AddCardValue("ActivationFilter", {l.Units}, cs.BonusPower, 15000, p.UntilEndOfTurn)
	end
	if (obj.GetNumberOf("SapphireFilter1", {l.PlayedOrdersThisTurn}) > 0 or obj.GetNumberOf("SapphireFilter2", {l.SoulBlasted}) > 0) and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.Stand("ActivationFilter")
		obj.Shuffle()
	end
end

function RubyFilter1(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_055")
end

function RubyFilter2(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_055") and snapshot.abilitySource.location == l.VC
end

function SapphireFilter1(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_116")
end

function SapphireFilter2(snapshot)
	return snapshot.name == obj.GetNameFromCardID("dlbt01_116") and snapshot.SourceIsAbility() and snapshot.abilitySource.location == l.VC
end

function ActivationFilter(id)
	return obj.IsThis(id) and obj.IsSameZone(id)
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSendToBottom("CostFilter", {l.Units}, 1) end
	obj.CounterBlast(1)
	obj.SendToBottom("CostFilter", {l.Units}, 1)
end

function CostFilter(id)
	return obj.IsRearguard(id) and obj.IsCircle(id, obj.Convert(FL.PlayerBackCenter))
end