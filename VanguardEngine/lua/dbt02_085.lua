-- Twisting Bulldoze

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC
	elseif n == 2 then
		return q.Location, l.EnemyRC
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.GetNumberOf(1) > obj.GetNumberOf(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SoulCharge(1)
		obj.AddBattleOnlyPower(3, 10000)
	end
	return 0
end