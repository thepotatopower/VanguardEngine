-- Aurora Battle Princess, Agra Rouge

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerPrisoners, q.Count, 2
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.FrontRowEnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.PlacedOnRC, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() or obj.IsGuardian() then
			return true
		end
	elseif n == 2 then
		if obj.LastPlacedOnRC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.HasPrison() and obj.Exists(3) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.Exists(1) then
			obj.SetAbilityPower(2, 5000)
			obj.SetAbilityShield(2, 10000)
		end
	elseif n == 2 then
		obj.ChooseImprison(3)
	end
	return 0
end