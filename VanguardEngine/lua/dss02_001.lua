-- 封焔竜 アウシュニヤ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return (obj.IsSameZone() and obj.GetNumberOf("Filter1", {l.VCArms}) > 0) or (obj.GetNumberOf("Filter2", {l.VCArms}) > 0 and obj.CanPayCost("Cost"))
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Filter1(id)
	return obj.IsOrderType(id, ot.RightDeityArms)
end

function Filter2(id)
	return obj.IsOrderType(id, ot.LeftDeityArms)
end

function Activation()
	if obj.IsSameZone() and obj.GetNumberOf("Filter1", {l.VCArms}) > 0 then
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
	if obj.GetNumberOf("Filter2", {l.VCArms}) > 0 and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.SuperiorCall({q.Location, l.Drop, q.Grade, 1, q.Other, o.GradeOrLess, q.Count, 1})
	end
end