-- 焔の巫女 アメリア

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Nirvana")}) and obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanAddToSoul({q.Other, o.This}) end
	obj.CounterBlast(1)
	obj.AddToSoul({q.Other, o.This})
end

function CanFullyResolve()
	return obj.CanSuperiorRide({q.Location, l.Hand, q.NameContains, obj.LoadName("Nirvana"), q.Other, o.Unit})
end

function Activation()
	obj.SuperiorRide({q.Location, l.Hand, q.NameContains, obj.LoadName("Nirvana"), q.Other, o.Unit})
	obj.AddPlayerState(ps.CannotSuperiorRide, p.UntilEndOfTurn)
end