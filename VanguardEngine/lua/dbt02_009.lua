-- Sylvan Horned Beast, Damainaru

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.SB, 1
	elseif n == 2 then
		return a.OnChosen, p.OncePerTurn, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Sylvan Horned Beast King, Magnolia") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.ChosenByVanguard() then
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
		obj.Select(1)
		obj.AllowBackRowAttack(3)
		obj.AddTempPower(3, 5000)
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(2, 5000)
	end
	return 0
end
