-- Inheritance Maiden, Hendrina

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, p.Retire, 1
	end
end

function CheckCondition(n)
	if n == 1 then 
		if obj.LastPlacedOnRC() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then 
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Mill(3)
	elseif n == 2 then
		obj.AlchemagicFreeSB()
	end
	return 0
end