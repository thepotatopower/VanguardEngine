-- Inheritance Maiden, Hendrina

function NumberOfAbilities()
	return 1
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
		return q.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return q.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
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

function Cost(n)
	if n == 2 then
		obj.Retire(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Mill(3)
	elseif n == 2 then
		obj.AlchemagicFreeSB();
	end
	return 0
end