-- バシュフルストライバー キンキー

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.CanAddToSoul({q.Location, l.Drop, q.NameContains, obj.LoadName("Astesice"), q.Grade, 2, q.UnitType, u.Normal, q.Other, o.GradeOrLess})
end

function Activation()
	obj.Store(obj.ChooseAddToSoul({q.Location, l.Drop, q.NameContains, obj.LoadName("Astesice"), q.Grade, 2, q.UnitType, u.Normal, q.Other, o.GradeOrLess, q.Count, 1}))
	if obj.Exists({q.Location, l.Stored}) and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end