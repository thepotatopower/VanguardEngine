-- Gunning of Dust Storm, Nigel

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	elseif n == 2 then
		return a.OnEnemyRetired, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Cataclysmic Bullet of Dust Storm, Randor") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.PlayerMainPhase() and obj.CanB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.CanRetire(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(1)
		obj.Retire(3)
	end
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.ChooseRetire(2)
	end
	return 0
end