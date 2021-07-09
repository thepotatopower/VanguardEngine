-- Grand Heavenly Sword, Alden

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 7
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 2
	elseif n == 2 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 4
	elseif n == 5 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 6 then
		return q.Location, l.Selected, q.Count, 1
	elseif n == 7 then
		return q.Location, l.Selected, q.Grade, 3, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnRC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnRC() and obj.CanCB(1) and obj.CanSB(2) and obj.Exists(3) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.Exists(4) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(3) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
		obj.SoulBlast(2)
	end
end

function Activate(n)
	if n == 1 then
		obj.Select(3)
		obj.SuperiorCall(6)
		if obj.Exists(7) then
			obj.Draw(2)
		end
		obj.EndSelect()
	elseif n == 2 then
		obj.AddTempPower(5, 5000)
	end
	return 0
end