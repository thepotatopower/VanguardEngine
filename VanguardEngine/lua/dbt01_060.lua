-- Gunning of Dust Storm, Nigel

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, p.HasPrompt, p.IsMandatory
	elseif n == 2 then
		return a.OnEnemyRetired, p.HasPrompt, p.CB, 1, p.Retire, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Cataclysmic Bullet of Dust Storm, Randor") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.PlayerMainPhase() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.CanRetire(1) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
	elseif n == 2 then
		obj.ChooseRetire(1)
	end
	return 0
end