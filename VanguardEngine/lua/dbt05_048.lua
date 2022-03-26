-- 颯爽の騎士 フリーダ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable() and obj.PlayerMainPhase() 
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Blaster")})
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function Activation()
	obj.LookAtTopOfDeck(5)
	obj.DisplayCards({q.Location, l.Looking})
	obj.Store(obj.SuperiorCall({q.Location, l.Looking, q.Grade, 2, q.NameContains, obj.LoadName("Blaster"), q.Other, o.GradeOrLess, q.Other, o.Unit, q.Count, 1, q.Min, 0}))
	obj.Shuffle()
	if obj.Exists({q.Location, l.Stored}) then
		obj.Discard(1)
	end
end
