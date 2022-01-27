-- 鎧穿の騎士 ムーゲン

function RegisterAbilities()
	-- on place
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
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Blaster")})
end

function Cost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function Activation()
	obj.LookAtTopOfDeck(2)
	obj.SuperiorCallAsRest({q.Location, l.Looking, q.Other, o.Unit, q.Count, 2, q.Min, 0})
	obj.AddToDrop({q.Location, l.Looking})
end