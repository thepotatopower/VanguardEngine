-- Grand Heavenly Sword, Alden

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Grade, 3, q.Count, 3
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Selected, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Selected, q.Grade, 3, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, p.HasPrompt, p.CB, 2, p.SB, 1
	elseif n == 2 then
		return a.OnAttack, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(2) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Select(1)
		if obj.Exists(5) and obj.SuperiorCall(5) then
			obj.Draw(2)
		end
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
	end
	return 0
end