-- 幽魂の抱擁 ベティーナ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.OncePerTurn)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Race, r.Ghost, q.Other, o.NotThis, q.Other, o.SameColumn})
end

function Condition()
	return obj.Exists({q.Location, l.Drop, q.NameContains, obj.LoadName("Feltyrosa"), q.Count, 1})
end

function Activation()
	obj.ChooseAddToSoul({q.Location, l.Drop, q.NameContains, obj.LoadName("Feltyrosa"), q.Count, 1})
end