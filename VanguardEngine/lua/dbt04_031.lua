-- 焔の蹴僧 コウケイ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.CanSuperiorCallToSpecificCircle({q.Location, l.Drop, q.Grade, 0, q.Count, 1}, obj.GetColumn({q.Other, o.ThisFieldID}))
end

function Activation()
	obj.SuperiorCallToSpecificCircle({q.Location, l.Drop, q.Grade, 0, q.Count, 1}, obj.GetColumn({q.Other, o.ThisFieldID}))
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusSkills, s.Boost, p.UntilEndOfTurn)
end