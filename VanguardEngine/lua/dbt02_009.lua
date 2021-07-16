-- Sylvan Horned Beast, Damainaru

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	elseif n == 2 then
		return a.OnChosen, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.VanguardIs("Sylvan Horned Beast King, Magnolia") and obj.CanSB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and not obj.Activated() and obj.ChosenByVanguard() then
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

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(2)
		obj.AllowBackRowAttack(4)
		obj.AddTempPower(4, 5000)
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
	end
	return 0
end
