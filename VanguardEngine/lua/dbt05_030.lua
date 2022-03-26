-- 忠義の騎士 ベディヴィア

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function Activation()
	obj.SuperiorCall({"Filter"}, {l.Deck}, 1, 0, "", {}, {obj.GetColumn({q.Other, o.ThisFieldID})})
	obj.Shuffle()
end

function Filter(id)
	return obj.NameIs(id, obj.GetNameFromCardID("dbt05_033"))
end